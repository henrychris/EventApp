using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.DTO;
using UserAPI.Interfaces;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errMessage = string.Join(" | ", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));
                    return BadRequest(errMessage);
                }

                var result = await _authService.RegisterAsync(request);

                if (result.Success)
                {
                    return Ok($"Status: {result.Success}.\nMessage: {result.Message}\nData: {result}.");
                }
                return BadRequest($"Status: {result.Success}.\nMessage: {result.Message}\n");
            }
            catch (Exception ex)
            {
                var errMessage = string.Concat($"Message: {ex.Message}", "\n", $"Stack Trace: {ex.StackTrace}");
                return BadRequest($"Error: {errMessage}");
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errMessage = string.Join(" | ", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));
                    return BadRequest(errMessage);
                }

                var result = await _authService.LoginAsync(request);

                if (result.Success)
                {
                    return Ok($"Status: {result.Success}.\nMessage: {result.Message}\nData: {JsonConvert.SerializeObject(result)}.");
                }
                return BadRequest($"Status: {result.Success}.\nMessage: {result.Message}\n");
            }
            catch (Exception ex)
            {
                var errMessage = string.Concat($"Message: {ex.Message}", "\n", $"Stack Trace: {ex.StackTrace}");
                return BadRequest($"Error: {errMessage}");
            }
        }
    }
}
