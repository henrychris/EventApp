using Microsoft.EntityFrameworkCore;
using Shared;
using UserAPI.Data;
using UserAPI.Interfaces;

namespace UserAPI.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
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
        /// Get an ICollection of all events a user is attending.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>An ICollection of Event type.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public ICollection<Event> GetEventsUserIsAttending(string userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Using their email address, fetch a user's details.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Returns user details if found, else returns a default user</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(c => c.Email == email) ?? User.NotFound;
        }

        public new async Task<IEnumerable<object>> GetAllAsync()
        {
            var users = await _context.Set<User>().Select(u => new 
            {
                u.FirstName,
                u.LastName,
                u.Email,
                u.WalletBalance,
                u.Role
            }).ToListAsync();

            return users;
        }
    }
}
