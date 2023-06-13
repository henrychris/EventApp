using Shared;
using Shared.Repository;
using Shared.ResponseModels;
using UserAPI.Data;

namespace UserAPI.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> CheckIfUserExistsAsync(string email);
        Task<User> GetUserAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        ICollection<Event> GetEventsUserIsAttending(string userId);
    }
}
