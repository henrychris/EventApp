using AutoMapper;
using Shared;
using Shared.DTO;
using System.Security.Claims;
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
        public Task<ServiceResponse<User>> LoginAsync(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<User>> RegisterAsync(RegisterRequest registerRequest)
        {
            // dont forget to add a try-catch block in the calling controller.
            ClaimsIdentity identity = new();
            if (registerRequest == null)
            {
                return new ServiceResponse<User> { Data = User.NotFound, Message = "Failed to register user.", Success = false };
            }

            if (!await _userRepo.CheckIfUserExistsAsync(registerRequest.Email))
            {
                return new ServiceResponse<User> { Data = User.NotFound, Message = $"{registerRequest.Email} already exists.", Success = false };
            }

            if (registerRequest.Role != Roles.User.ToString() || registerRequest.Role != Roles.Admin.ToString())
            {
                return new ServiceResponse<User> { Data = User.NotFound, Message = "Role does not exist.", Success = false };
            }

            var passwordHashObj = _hasher.HashPasword(registerRequest.Password);

            User user = _mapper.Map<User>(registerRequest);
            user.PasswordHash = passwordHashObj.Hash;
            user.PasswordSalt = passwordHashObj.Salt;

            var addResponse = await _userRepo.AddUserAsync(user);

            if (!addResponse.Success)
            {
                return new ServiceResponse<User> { Data = User.NotFound, Message = addResponse.Message, Success = false };
            }

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            return new ServiceResponse<User> { Success = true, Message = addResponse.Message, Data = user };
        }
    }
}
