using Shared;
using Shared.ResponseModels;

namespace UserAPI.Interfaces
{
    public interface IJwtManager
    {
        ServiceResponse<string> CreateAuthToken(User user);
    }
}
