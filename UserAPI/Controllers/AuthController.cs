﻿using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
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
                return Ok($"Status: {result.Success}.\nMessage: {result.Message}\nData: {result}.");
            }
            return Unauthorized($"Status: {result.Success}.\nMessage: {result.Message}\n");

        }
    }
}
