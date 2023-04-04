using Shared;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<ServiceResponse<User>> AddUserAsync(User user);
        Task<bool> CheckIfUserExistsAsync(string email);
        Task<User> GetUserAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        ICollection<Event> GetEventsUserIsAttending(int userId);
        Task<ServiceResponse<User>> FundWalletAsync(int userId, decimal amount);
    }
}
