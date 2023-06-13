using Shared.ResponseModels;
using Shared;

namespace UserAPI.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<User>> AddUserAsync(User user);
        Task<bool> VerifyUserPasswordAsync(string email, string password);
        Task<ServiceResponse<User>> FundWalletAsync(string userId, decimal amount);
    }
}
