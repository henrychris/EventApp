using Shared;

namespace UserAPI.Interfaces
{
    public interface IUserService
    {
        public Task<bool> VerifyUserPasswordAsync(string email, string password);
    }
}
