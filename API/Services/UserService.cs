using API.Interfaces;
using Shared;
using System.Text;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly ISecurePasswordHasher _hasher;

        public UserService(IUserRepository userRepo, ISecurePasswordHasher hasher)
        {
            _userRepo = userRepo;
            _hasher = hasher;
        }

        /// <summary>
        /// Use the SecurePasswordHasher service to check if a given password matches a user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns></returns>
        public async Task<bool> VerifyUserPasswordAsync(string email, string password)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            return _hasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        }
    }
}
