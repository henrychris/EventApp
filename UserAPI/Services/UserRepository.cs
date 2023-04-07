using Microsoft.EntityFrameworkCore;
using Shared;
using UserAPI.Data;
using UserAPI.Interfaces;

namespace UserAPI.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Add a new user to the DB.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns user details in response.</returns>
        public async Task<ServiceResponse<User>> AddUserAsync(User user)
        {
            if (user != null)
            {
                await _dataContext.Users.AddAsync(user);
                await _dataContext.SaveChangesAsync();
                return new ServiceResponse<User> { Message = "User Added.", Success = true, Data = user };
            }
            else if (await CheckIfUserExistsAsync(user!.Email) && user != null)
            {
                return new ServiceResponse<User> { Message = "User already exists.", Success = false, Data = User.NotFound };
            }
            return new ServiceResponse<User> { Success = false, Message = "Failed to Add User.", Data = User.NotFound };
        }

        /// <summary>
        /// Check if a user exists in the DB.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True if user exists, False if not found.</returns>
        public async Task<bool> CheckIfUserExistsAsync(string email)
        {
            return await _dataContext.Users.AnyAsync(c => c.Email == email.ToLower());
        }

        /// <summary>
        /// Fund a user's wallet with a given amount.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="amount"></param>
        /// <returns>Returns user details in response.</returns>
        public async Task<ServiceResponse<User>> FundWalletAsync(int userId, decimal amount)
        {
            var user = await GetUserAsync(userId);

            if (user == null)
            {
                return new ServiceResponse<User> { Message = "Credit failed. User Not Found.", Success = false, Data = User.NotFound };
            }
            // The default, or NotFound user is an empty object.
            else if (user == User.NotFound)
                return new ServiceResponse<User> { Message = "User Not Found", Success = false, Data = User.NotFound };

            user.WalletBalance += amount;
            await _dataContext.SaveChangesAsync();

            return new ServiceResponse<User>
            {
                Message = $"Wallet credited with NGN{amount}. New balance: {user.WalletBalance}. Old Balance: {user.WalletBalance - amount}.",
                Success = true,
                Data = user
            };
        }

        /// <summary>
        /// Get an ICollection of all events a user is attending.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>An ICollection of Event type.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public ICollection<Event> GetEventsUserIsAttending(int userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a user using the user's ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns user details if found, else returns default user</returns>
        public async Task<User> GetUserAsync(int userId)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(c => c.Id == userId) ?? User.NotFound;
        }

        /// <summary>
        /// Using their email address, fetch a user's details.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Returns user details if found, else returns a default user</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (!await CheckIfUserExistsAsync(email))
                return User.NotFound;

            return await _dataContext.Users.FirstOrDefaultAsync(c => c.Email == email) ?? User.NotFound;
        }
    }
}
