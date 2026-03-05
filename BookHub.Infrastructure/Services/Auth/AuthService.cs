using BookHub.Core.DTOs.Auth;
using BookHub.Core.Entities;
using BookHub.Core.Exceptions;
using BookHub.Core.Interfaces.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookHub.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _logger = logger;
        }

        public async Task Register(RegisterDto registerDto)
        {
            _logger.LogInformation("Attempting to register new user: {UserName}", registerDto.UserName);

            var user = new ApplicationUser { UserName = registerDto.UserName, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Registration failed for {UserName}: {Errors}", registerDto.UserName, errors);
                throw new BadRequestException($"User registration failed: {errors}");
            }

            var isFirstUser = !_userManager.Users.Any(u => u.Id != user.Id);
            var roleToAssign = isFirstUser ? "Admin" : "User";
            await _userManager.AddToRoleAsync(user, roleToAssign);

            _logger.LogInformation("User {UserName} registered successfully with role {Role}", user.UserName, roleToAssign);
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed: User with email {Email} not found.", loginDto.Email);
                throw new UnauthorizedException("Invalid credentials");
            }

            var passwordValid = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!passwordValid.Succeeded)
            {
                _logger.LogWarning("Login failed: Invalid password for user {Email}", loginDto.Email);
                throw new UnauthorizedException("Invalid credentials");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.UserName!),
                new (ClaimTypes.Email, user.Email!)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            _logger.LogInformation("User {Email} logged in successfully. Token generated.", user.Email);

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email!,
                UserName = user.UserName!
            };
        }
    }
}
