using BookHub.Core.DTOs.Auth;

namespace BookHub.Infrastructure.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task Register(RegisterDto registerDto);
    }
}