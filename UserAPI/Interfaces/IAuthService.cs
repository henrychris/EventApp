using Shared;
using Shared.DTO;

namespace UserAPI.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<AuthResponse>> RegisterAsync(RegisterRequest authRequest);
        Task<ServiceResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest);

    }
}
