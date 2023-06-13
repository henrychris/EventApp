using Shared.RequestModels;
using Shared.ResponseModels;

namespace UserAPI.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<AuthResponse>> RegisterAsync(RegisterRequest authRequest);
        Task<ServiceResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest);

    }
}
