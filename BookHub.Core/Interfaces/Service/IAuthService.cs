using BookHub.Core.DTOs.Auth;

namespace BookHub.Core.Interfaces.Service
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task Register(RegisterDto registerDto);
    }
}