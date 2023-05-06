using Shared;
using Shared.DTO;

namespace UserAPI.Interfaces
{
    public interface IJwtManager
    {
        ServiceResponse<string> CreateAuthToken(User user);
    }
}
