﻿using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.ResponseModels;
using UserAPI.Data;
using UserAPI.Interfaces;

namespace UserAPI.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext) : base(dataContext)
        {
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
        /// Get a user using the user's ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns user details if found, else returns default user</returns>
        public async Task<User> GetUserAsync(string userId)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(c => c.Id == userId) ?? User.NotFound;
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
    }
}
