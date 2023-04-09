using AutoMapper;
using Shared;
using Shared.DTO;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using UserAPI.Interfaces;

namespace UserAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly ISecurePasswordHasher _hasher;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepo, ISecurePasswordHasher hasher, IMapper mapper)
        {
            _userRepo = userRepo;
            _hasher = hasher;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest)
        {
            if (!await _userRepo.CheckIfUserExistsAsync(loginRequest.Email))
            {
                return new ServiceResponse<AuthResponse> { Data = AuthResponse.NotFound, Message = $"Email or Password incorrect.", Success = false };
            }

            var user = await _userRepo.GetUserByEmailAsync(loginRequest.Email);
            var isPasswordCorrect = _hasher.VerifyPassword(loginRequest.Password, user!.PasswordHash, user.PasswordSalt);

            if (!isPasswordCorrect)
            {
                return new ServiceResponse<AuthResponse> { Data = AuthResponse.NotFound, Message = "Incorrect password, try again.", Success = false };
            }
            var response = _mapper.Map<AuthResponse>(user);
            return new ServiceResponse<AuthResponse> { Success = true, Message = $"Login successful. Welcome, {user.Email}.", Data = response };
        }

        public async Task<ServiceResponse<AuthResponse>> RegisterAsync(RegisterRequest registerRequest)
        {
            // dont forget to add a try-catch block in the calling controller.
            //ClaimsIdentity identity = new();
            if (registerRequest == null)
            {
                return new ServiceResponse<AuthResponse> { Data = AuthResponse.NotFound, Message = "Failed to register user.", Success = false };
            }

            if (await _userRepo.CheckIfUserExistsAsync(registerRequest.Email))
            {
                return new ServiceResponse<AuthResponse> { Data = AuthResponse.NotFound, Message = $"{registerRequest.Email} already exists.", Success = false };
            }

            // change this to store Roles in DB. Create Role Controller for managing the Role DB.
            if (registerRequest.Role != Roles.User.ToString().ToLower() && registerRequest.Role != Roles.Admin.ToString().ToLower())
            {
                return new ServiceResponse<AuthResponse> { Data = AuthResponse.NotFound, Message = "Role does not exist.", Success = false };
            }

            var passwordHashObj = _hasher.HashPasword(registerRequest.Password);

            User user = _mapper.Map<User>(registerRequest);
            user.PasswordHash = passwordHashObj.Hash;
            user.PasswordSalt = passwordHashObj.Salt;

            var addResponse = await _userRepo.AddUserAsync(user);

            if (!addResponse.Success)
            {
                return new ServiceResponse<AuthResponse> { Data = AuthResponse.NotFound, Message = addResponse.Message, Success = false };
            }

            //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            //identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            //identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
            var response = _mapper.Map<AuthResponse>(user);
            return new ServiceResponse<AuthResponse> { Success = true, Message = addResponse.Message, Data = response };
        }
    }
}
