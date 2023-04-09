using Shared;

namespace UserAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<ServiceResponse<User>> AddUserAsync(User user);
        Task<bool> CheckIfUserExistsAsync(string email);
        Task<User> GetUserAsync(string userId);
        Task<User?> GetUserByEmailAsync(string email);
        ICollection<Event> GetEventsUserIsAttending(string userId);
        Task<ServiceResponse<User>> FundWalletAsync(string userId, decimal amount);
    }
}
