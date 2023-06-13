using AutoMapper;
using Shared;
using Shared.RequestModels;
using Shared.ResponseModels;
using UserAPI.Interfaces;

namespace UserAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecurePasswordHasher _hasher;
        private readonly IMapper _mapper;
        private readonly IJwtManager _jwtManager;
        private readonly IUserService _userService;

        public AuthService(IUnitOfWork unitOfWork, ISecurePasswordHasher hasher, IMapper mapper, IJwtManager jwtManager, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _hasher = hasher;
            _mapper = mapper;
            _jwtManager = jwtManager;
            _userService = userService;
        }

        /// <summary>
        /// Logs in a user by checking their email and password.
        /// If the user exists and their password is correct, an authentication token is created and returned.
        /// <para>Otherwise, an error message is returned.</para>
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(loginRequest.Email);

            if (user == User.NotFound)
            {
                return new ServiceResponse<AuthResponse> { Data = AuthResponse.NotFound, Message = $"Email or Password incorrect.", Success = false };
            }
            var isPasswordCorrect = _hasher.VerifyPassword(loginRequest.Password, user.PasswordHash, user.PasswordSalt);

            if (isPasswordCorrect)
            {
                return new ServiceResponse<AuthResponse> { Data = null, Message = "Incorrect password, try again.", Success = false };
            }
            var response = _mapper.Map<AuthResponse>(user);
            response.Token = _jwtManager.CreateAuthToken(user).Data;
            return new ServiceResponse<AuthResponse> { Success = true, Message = $"Login successful. Welcome, {user.Email}.", Data = response };
        }

        public async Task<ServiceResponse<AuthResponse>> RegisterAsync(RegisterRequest registerRequest)
        {
            if (registerRequest == null)
            {
                return new ServiceResponse<AuthResponse> { Data = AuthResponse.NotFound, Message = "Failed to register user.", Success = false };
            }

            if (await _unitOfWork.Users.CheckIfUserExistsAsync(registerRequest.Email))
            {
                return new ServiceResponse<AuthResponse> { Data = AuthResponse.NotFound, Message = $"{registerRequest.Email} is already in use.", Success = false };
            }

            // TODO change this to store Roles in DB. Create Role repository for accessing the Role table.
            // It should be part of the DB init script to add the Roles to database.
            // when registering, pass in the roleId and use to avoid mispellings of roles
            // or use the enums. Find out which is best practice.
            if (!Enum.TryParse<UserRoles>(registerRequest.Role, true, out _))
            {
                return new ServiceResponse<AuthResponse> { Data = AuthResponse.NotFound, Message = "Role does not exist.", Success = false };
            }

            var passwordHashObj = _hasher.HashPasword(registerRequest.Password);

            User user = _mapper.Map<User>(registerRequest);
            user.PasswordHash = passwordHashObj.Hash;
            user.PasswordSalt = passwordHashObj.Salt;

            var addResponse = await _userService.AddUserAsync(user);

            if (!addResponse.Success)
            {
                return new ServiceResponse<AuthResponse> { Data = AuthResponse.NotFound, Message = addResponse.Message, Success = false };
            }

            var response = _mapper.Map<AuthResponse>(user);
            return new ServiceResponse<AuthResponse> { Success = true, Message = addResponse.Message, Data = response };
        }
    }
}
