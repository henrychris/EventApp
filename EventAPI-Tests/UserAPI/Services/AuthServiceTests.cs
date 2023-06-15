using AutoMapper;
using Moq;
using NUnit.Framework;
using Shared;
using Shared.RequestModels;
using Shared.ResponseModels;
using UserAPI;
using UserAPI.Interfaces;
using UserAPI.Services;

namespace EventAPI_Tests.UserAPI.Services
{
    public class AuthServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ISecurePasswordHasher> _passwordHasherMock;
        private Mock<IJwtManager> _jwtManagerMock;
        private Mock<IUserService> _userServiceMock;
        private IMapper _mapper;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _passwordHasherMock = new Mock<ISecurePasswordHasher>();
            _jwtManagerMock = new Mock<IJwtManager>();
            _userServiceMock = new Mock<IUserService>();
            _mapper = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            }).CreateMapper();
            _authService = new AuthService(_unitOfWorkMock.Object, _passwordHasherMock.Object, _mapper, _jwtManagerMock.Object, _userServiceMock.Object);
        }

        [Test]
        public async Task LoginAsync_WithCorrectCredentials_ReturnsSuccessfulResponseWithAuthToken()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password"
            };
            var user = new User
            {
                Email = loginRequest.Email,
                PasswordHash = "hashedPassword",
                PasswordSalt = new byte[] { 1, 2, 3, 4 }
            };
            var authToken = "authToken";

            _unitOfWorkMock.Setup(u => u.Users.GetUserByEmailAsync(loginRequest.Email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.VerifyPassword(loginRequest.Password, user.PasswordHash, user.PasswordSalt)).Returns(true);
            _jwtManagerMock.Setup(j => j.CreateAuthToken(user)).Returns(new ServiceResponse<string> { Success = true, Data = authToken });

            // Act
            var response = await _authService.LoginAsync(loginRequest);

            // Assert         
            Assert.That(response.Success, Is.True);
            Assert.That(response.Message, Is.EqualTo($"Login successful. Welcome, {user.Email}."));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Token, Is.EqualTo(authToken));
        }

        [Test]
        public async Task LoginAsync_WithNonExistingUser_ReturnsNotFoundResponse()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password"
            };

            User? user = null;

            _unitOfWorkMock.Setup(u => u.Users.GetUserByEmailAsync(loginRequest.Email))!.ReturnsAsync(user);

            // Act
            var response = await _authService.LoginAsync(loginRequest);

            // Assert
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo($"Email or Password incorrect."));
            Assert.That(response.Data, Is.EqualTo(AuthResponse.NotFound));
        }

        [Test]
        public async Task LoginAsync_WithIncorrectPassword_ReturnsIncorrectPasswordResponse()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password"
            };
            var user = new User
            {
                Email = loginRequest.Email,
                PasswordHash = "hashedPassword",
                PasswordSalt = new byte[] { 1, 2, 3, 4 }
            };

            _unitOfWorkMock.Setup(u => u.Users.GetUserByEmailAsync(loginRequest.Email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.VerifyPassword(loginRequest.Password, user.PasswordHash, user.PasswordSalt)).Returns(false);

            // Act
            var response = await _authService.LoginAsync(loginRequest);

            // Assert
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo("Incorrect password, try again."));
            Assert.That(response.Data, Is.Null);
        }

        [Test]
        public async Task RegisterAsync_WithValidRequest_ReturnsSuccessfulResponse()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                FirstName = "Henry",
                LastName = "Ford",
                Email = "test@example.com",
                Password = "password",
                Role = "User"
                // Set other properties as needed
            };
            var hashedPassword = "hashedPassword";
            var salt = new byte[] { 1, 2, 3, 4 };
            var user = _mapper.Map<User>(registerRequest);

            _unitOfWorkMock.Setup(u => u.Users.CheckIfUserExistsAsync(registerRequest.Email)).ReturnsAsync(false);
            _passwordHasherMock.Setup(h => h.HashPasword(registerRequest.Password)).Returns(new PasswordHash { Hash = hashedPassword, Salt = salt });
            _userServiceMock.Setup(u => u.AddUserAsync(It.IsAny<User>())).ReturnsAsync(new ServiceResponse<User> { Success = true, Data = user, Message = "User added successfully." });

            // Act
            var response = await _authService.RegisterAsync(registerRequest);

            // Assert
            Assert.That(response.Success, Is.True);
            Assert.That(response.Message, Is.EqualTo("User added successfully."));
            Assert.That(response.Data, Is.Not.Null);
            // Add additional assertions as needed
        }

        [Test]
        public async Task RegisterAsync_WithNullRequest_ReturnsFailedResponse()
        {
            // Act
            var response = await _authService.RegisterAsync(null!);

            // Assert
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo("Failed to register user."));
            Assert.That(response.Data, Is.EqualTo(AuthResponse.NotFound));
        }

        [Test]
        public async Task RegisterAsync_WithExistingEmail_ReturnsEmailInUseResponse()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                FirstName = "Henry",
                LastName = "Ford",
                Email = "test@example.com",
                Password = "password",
                Role = "User"
                // Set other properties as needed
            };

            _unitOfWorkMock.Setup(u => u.Users.CheckIfUserExistsAsync(registerRequest.Email)).ReturnsAsync(true);

            // Act
            var response = await _authService.RegisterAsync(registerRequest);

            // Assert
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo($"{registerRequest.Email} is already in use."));
            Assert.That(response.Data, Is.EqualTo(AuthResponse.NotFound));
        }

        [Test]
        public async Task RegisterAsync_WithInvalidRole_ReturnsRoleNotFoundResponse()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                FirstName = "Henry",
                LastName = "Ford",
                Email = "test@example.com",
                Password = "password",
                Role = "InvalidRole"
                // Set other properties as needed
            };

            _unitOfWorkMock.Setup(u => u.Users.CheckIfUserExistsAsync(registerRequest.Email)).ReturnsAsync(false);

            // Act
            var response = await _authService.RegisterAsync(registerRequest);

            // Assert
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo("Role does not exist."));
            Assert.That(response.Data, Is.EqualTo(AuthResponse.NotFound));
        }
    }
}
