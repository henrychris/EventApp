using Shared;
using Shared.DTO;
using Shared.ResponseModels;

namespace UserAPI.Interfaces
{
    public interface IJwtManager
    {
        ServiceResponse<string> CreateAuthToken(User user);
    }
}
