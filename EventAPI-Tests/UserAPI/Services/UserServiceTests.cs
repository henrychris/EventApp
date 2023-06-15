using Moq;
using NUnit.Framework;
using Shared;
using UserAPI.Interfaces;
using UserAPI.Services;

namespace EventAPI_Tests.UserAPI.Services
{
    public class UserServiceTests
    {
        private IUserService _userService;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ISecurePasswordHasher> _hasherMock;

        [SetUp]
        public void Setup()
        {
            // Create mock instances of dependencies
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _hasherMock = new Mock<ISecurePasswordHasher>();

            // Create an instance of the UserService with mock dependencies
            _userService = new UserService(_unitOfWorkMock.Object, _hasherMock.Object);
        }

        [Test]
        public async Task AddUserAsync__ValidUser_ReturnsSuccessResponseWithData()
        {
            // Arrange
            var user = new User
            {
                FirstName = "Henry",
                LastName = "Ford",
                Email = "henry@ford.com",
                PasswordHash = "passwordhash"
            };

            _unitOfWorkMock.Setup(u => u.Users.CheckIfUserExistsAsync(user.Email)).ReturnsAsync(false);

            // Act
            var response = await _userService.AddUserAsync(user);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.Success, Is.True);
                Assert.That(response.Message, Is.EqualTo("User Added."));
                Assert.That(response.Data, Is.EqualTo(user));
            });
        }

        [Test]
        public async Task AddUserAsync_NullUser_ReturnsFailureResponse()
        {
            // Act
            var response = await _userService.AddUserAsync(null);

            // Assert
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo("Failed to Add User."));
            Assert.That(response.Data, Is.EqualTo(null));
        }

        [Test]
        public async Task AddUserAsync_ExistingUser_ReturnsFailureResponse()
        {
            // Arrange
            var existingUser = new User { Email = "existing@example.com" };
            _unitOfWorkMock.Setup(u => u.Users.CheckIfUserExistsAsync(existingUser.Email)).ReturnsAsync(true);

            // Act
            var response = await _userService.AddUserAsync(existingUser);

            // Assert
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo("User already exists."));
            Assert.That(response.Data, Is.EqualTo(null));
        }

        [Test]
        public async Task FundWalletAsync_WithValidUserIdAndPositiveAmount_ReturnsSuccessfulResponse()
        {
            // Arrange 
            var user = new User
            {
                Id = "df2f2c1a-7a36-413b-9dac-6a138ed52b1a",
                FirstName = "Henry",
                LastName = "Ford",
                Email = "henry@ford.com"
            };
            var amount = 1000.00m;
            decimal oldBalance = user.WalletBalance;
            var expectedMessage = $"Wallet credited with NGN{amount}. New balance: {oldBalance + amount}. Old Balance: {oldBalance}.";
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            var response = await _userService.FundWalletAsync(user.Id, amount);

            // Assert
            Assert.That(response.Success, Is.True);
            Assert.That(response.Message, Is.EqualTo(expectedMessage));
            Assert.That(response.Data, Is.EqualTo(user));
        }

        [Test]
        public async Task FundWalletAsync_WithInvalidUserId_ReturnsUserNotFoundResponse()
        {
            // Arrange
            User? user = null;
            var userId = "df2f2c1a-7a36-413b-9dac-6a138ed52b1a";
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(userId))!.ReturnsAsync(user);

            // Act
            var response = await _userService.FundWalletAsync(userId, 1000.00m);

            // Assert
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo("User Not Found."));
            Assert.That(response.Data, Is.EqualTo(null));
        }

        [Test]
        public async Task FundWalletAsync_WithZeroAmount_ReturnsSuccessfulResponseWithZeroChange()
        {
            // Arrange 
            var user = new User
            {
                Id = "df2f2c1a-7a36-413b-9dac-6a138ed52b1a",
                FirstName = "Henry",
                LastName = "Ford",
                Email = "henry@ford.com"
            };
            var amount = 0.00m;
            var oldBalance = user.WalletBalance;
            var expectedMessage = $"Amount cannot be zero.";
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            var response = await _userService.FundWalletAsync(user.Id, amount);

            // Assert
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo(expectedMessage));
            Assert.That(response.Data, Is.EqualTo(user));
            Assert.That(response.Data.WalletBalance, Is.EqualTo(oldBalance));
        }

        [Test]
        public async Task FundWalletAsync_WithNegativeAmount_ReturnsErroResponse()
        {
            // Arrange 
            var user = new User
            {
                Id = "df2f2c1a-7a36-413b-9dac-6a138ed52b1a",
                FirstName = "Henry",
                LastName = "Ford",
                Email = ""
            };
            var amount = -1000.00m;
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            var response = await _userService.FundWalletAsync(user.Id, amount);

            // Assert
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo("Amount cannot be negative."));
            Assert.That(response.Data, Is.EqualTo(user));
        }

        [Test]
        public async Task FundWalletAsync_WithValidUserIdAndZeroInitialBalanceAndMaxDecimalAmount_ReturnsSuccessfulResponse()
        {
            // Arrange 
            var user = new User
            {
                Id = "df2f2c1a-7a36-413b-9dac-6a138ed52b1a",
                FirstName = "Henry",
                LastName = "Ford",
                Email = ""
            };
            var amount = decimal.MaxValue;
            decimal oldBalance = user.WalletBalance;
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            var response = await _userService.FundWalletAsync(user.Id, amount);

            // Assert
            Assert.That(response.Success, Is.True);
            Assert.That(response.Data.WalletBalance, Is.EqualTo(decimal.MaxValue));
            Assert.That(response.Data, Is.EqualTo(user));
        }

        [Test]
        public async Task UpdateUserAsync_WithValidIdAndUserData_ReturnsSuccessfulResponse()
        {
            // Arrange
            string userId = "123";
            var updateUserRequest = new UpdateUserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };
            var userToUpdate = new User
            {
                Id = userId,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                Email = "old.email@example.com"
            };
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(userId)).ReturnsAsync(userToUpdate);

            // Act
            var response = await _userService.UpdateUserAsync(userId, updateUserRequest);

            // Assert
            Assert.That(response.Success, Is.True);
            Assert.That(response.Message, Is.EqualTo("User Updated."));
            Assert.That(response.Data, Is.Null);

            Assert.That(userToUpdate.FirstName, Is.EqualTo(updateUserRequest.FirstName));
            Assert.That(userToUpdate.LastName, Is.EqualTo(updateUserRequest.LastName));
            Assert.That(userToUpdate.Email, Is.EqualTo(updateUserRequest.Email));

            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_WithInvalidId_ReturnsFailedResponseWithInvalidIdMessage()
        {
            // Arrange
            User? nullUser = null;
            var invalidId = "invalid-id";
            var updateUserRequest = new UpdateUserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(invalidId))!.ReturnsAsync(nullUser);
            // Act
            var response = await _userService.UpdateUserAsync(invalidId, updateUserRequest);

            // Assert
            Assert.That(response.Success, Is.False);
            Assert.That(response.Message, Is.EqualTo("User Not Found."));
            Assert.That(response.Data, Is.Null);
            _unitOfWorkMock.Verify(u => u.Users.GetByIdAsync(invalidId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Users.Update(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Test]
        public async Task UpdateUserAsync_WithSameUserData_ReturnsSuccessfulResponseWithNoUpdates()
        {
            // Arrange
            var userId = "user-id";
            var existingUser = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };
            var updateUserRequest = new UpdateUserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            // Act
            var response = await _userService.UpdateUserAsync(userId, updateUserRequest);

            // Assert
            Assert.That(response.Success, Is.True);
            Assert.That(response.Message, Is.EqualTo("User Updated."));
            Assert.IsNull(response.Data);
            Assert.That(existingUser.FirstName, Is.EqualTo("John"));
            Assert.That(existingUser.LastName, Is.EqualTo("Doe"));
            Assert.That(existingUser.Email, Is.EqualTo("john@example.com"));

            _unitOfWorkMock.Verify(u => u.Users.GetByIdAsync(userId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Users.Update(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    
        
    }
}
