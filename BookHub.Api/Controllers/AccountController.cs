using BookHub.Core.DTOs.Auth;
using BookHub.Core.Exceptions;
using BookHub.Core.Interfaces.Service;
using BookHub.Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

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
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            _logger.LogInformation("Received registration request for: {Email}", dto.Email);
            await _authService.Register(dto);
            return Ok(ApiResponse<string?>.Ok(message : "User registered successfully"));
        }

        [HttpPost("login")]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            _logger.LogInformation("Received login request for: {Email}", dto.Email);
            var authResponse = await _authService.Login(dto);
            return Ok(ApiResponse<AuthResponseDto>.Ok(authResponse, "Login successful"));
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new UnauthorizedException("User is not authenticated.");

            var profile = await _authService.GetProfile(userId);
            return Ok(ApiResponse<UserProfileDto>.Ok(profile, "Profile retrieved successfully"));
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new UnauthorizedException("User is not authenticated.");

            var updated = await _authService.UpdateProfile(userId, dto);
            return Ok(ApiResponse<UserProfileDto>.Ok(updated, "Profile updated successfully"));
        }

    }
}