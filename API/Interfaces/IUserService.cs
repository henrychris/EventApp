using Shared;

namespace API.Interfaces
{
    public interface IUserService
    {
        public Task<bool> VerifyUserPasswordAsync(string email, string password);
    }
}
