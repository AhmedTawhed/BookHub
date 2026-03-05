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
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthService authService, ILogger<AccountController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            _logger.LogInformation("Received registration request for: {Email}", dto.Email);
            await _authService.Register(dto);
            return Ok(ApiResponse<string?>.Ok(message : "User registered successfully"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            _logger.LogInformation("Received login request for: {Email}", dto.Email);
            var authResponse = await _authService.Login(dto);
            return Ok(ApiResponse<AuthResponseDto>.Ok(authResponse, "Login successful"));
        }
    }
}