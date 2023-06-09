﻿using Shared;
using Shared.Repository;

namespace UserAPI.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> CheckIfUserExistsAsync(string email);
        Task<User> GetUserByEmailAsync(string email);
        ICollection<Event> GetEventsUserIsAttending(string userId);

        new Task<IEnumerable<object>> GetAllAsync();
    }
}
