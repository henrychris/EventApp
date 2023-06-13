using Shared;
using Shared.ResponseModels;
using UserAPI.Interfaces;

namespace UserAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecurePasswordHasher _hasher;

        public UserService(IUnitOfWork unitOfWork, ISecurePasswordHasher hasher)
        {
            _unitOfWork = unitOfWork;
            _hasher = hasher;
        }

        /// <summary>
        /// Add a new user to the DB.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns user details in response.</returns>
        public async Task<ServiceResponse<User>> AddUserAsync(User user)
        {
            if (user == null)
                return new ServiceResponse<User> { Success = false, Message = "Failed to Add User.", Data = User.NotFound };
            
            else if (await _unitOfWork.Users.CheckIfUserExistsAsync(user.Email))
                return new ServiceResponse<User> { Message = "User already exists.", Success = false, Data = User.NotFound };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();
            return new ServiceResponse<User> { Message = "User Added.", Success = true, Data = user };
        }

        /// <summary>
        /// Use the SecurePasswordHasher service to check if a given password matches a user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns></returns>
        public async Task<bool> VerifyUserPasswordAsync(string email, string password)
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(email);

            if (user == null)
                return false;

            // passwordhash and salt will never be null when this function is called.
            return _hasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        }

        /// <summary>
        /// Fund a user's wallet with a given amount.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="amount"></param>
        /// <returns>Returns user details in response.</returns>
        public async Task<ServiceResponse<User>> FundWalletAsync(string userId, decimal amount)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null)
                return new ServiceResponse<User> { Message = "Credit failed. User Not Found.", Success = false, Data = User.NotFound };

            // The default, or NotFound user is an empty object.
            else if (user == User.NotFound)
                return new ServiceResponse<User> { Message = "User Not Found", Success = false, Data = User.NotFound };

            user.WalletBalance += amount;
            await _unitOfWork.CompleteAsync();

            return new ServiceResponse<User>
            {
                Message = $"Wallet credited with NGN{amount}. New balance: {user.WalletBalance}. Old Balance: {user.WalletBalance - amount}.",
                Success = true,
                Data = user
            };
        }
    }
}
