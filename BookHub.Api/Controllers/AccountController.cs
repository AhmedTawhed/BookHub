using BookHub.Core.DTOs.Auth;
using BookHub.Core.Interfaces.Service;
using BookHub.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            await _authService.Register(dto);
            return Ok(ApiResponse<string>.Ok(null, "User registered successfully"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var authResponse = await _authService.Login(dto);
            return Ok(ApiResponse<AuthResponseDto>.Ok(authResponse, "Login successful"));
        }
    }
}