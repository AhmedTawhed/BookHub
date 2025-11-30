using BookHub.Core.DTOs.Auth;
using BookHub.Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.Controllers
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _authService.Register(dto);
                return Ok(new { Message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.Login(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }
    }
}