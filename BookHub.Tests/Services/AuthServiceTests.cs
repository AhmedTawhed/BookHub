using BookHub.Contracts;
using BookHub.Core.DTOs.Auth;
using BookHub.Core.Entities;
using BookHub.Core.Exceptions;
using BookHub.Core.Interfaces;
using BookHub.Infrastructure.Services.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookHub.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IEventPublisher> _mockEventPublisher;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                _mockUserManager.Object,
                Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);

            _mockConfig = new Mock<IConfiguration>();
            _mockEventPublisher = new Mock<IEventPublisher>();

            _service = new AuthService(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockConfig.Object,
                Mock.Of<ILogger<AuthService>>(),
                _mockEventPublisher.Object);
        }

        [Fact]
        public async Task Register_ValidRequest_PublishesUserRegisteredEvent()
        {
            var dto = new RegisterDto { UserName = "ahmed", Email = "ahmed@test.com", Password = "Pass123!" };
            var capturedUser = new ApplicationUser { Id = "user-1", UserName = dto.UserName, Email = dto.Email };

            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<ApplicationUser, string>((u, _) =>
                {
                    u.Id = capturedUser.Id;
                    u.UserName = capturedUser.UserName;
                    u.Email = capturedUser.Email;
                });

            _mockUserManager.Setup(m => m.Users)
                .Returns(new List<ApplicationUser>().AsQueryable());

            _mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            await _service.Register(dto);

            _mockEventPublisher.Verify(
                p => p.PublishAsync(It.Is<UserRegisteredEvent>(e => e.UserName == dto.UserName && e.Email == dto.Email)),
                Times.Once);
        }

        [Fact]
        public async Task Register_IdentityFailure_ThrowsBadRequestException()
        {
            var dto = new RegisterDto { UserName = "ahmed", Email = "ahmed@test.com", Password = "weak" };

            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too short." }));

            Func<Task> act = async () => await _service.Register(dto);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*Password too short.*");

            _mockEventPublisher.Verify(p => p.PublishAsync(It.IsAny<UserRegisteredEvent>()), Times.Never);
        }

        [Fact]
        public async Task Login_InvalidEmail_ThrowsUnauthorizedException()
        {
            var dto = new LoginDto { Email = "nobody@test.com", Password = "Pass123!" };
            _mockUserManager.Setup(m => m.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);

            Func<Task> act = async () => await _service.Login(dto);

            await act.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid credentials");
        }

        [Fact]
        public async Task Login_InvalidPassword_ThrowsUnauthorizedException()
        {
            var dto = new LoginDto { Email = "ahmed@test.com", Password = "WrongPass!" };
            var user = new ApplicationUser { Id = "user-1", UserName = "ahmed", Email = dto.Email };

            _mockUserManager.Setup(m => m.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _mockSignInManager.Setup(m => m.CheckPasswordSignInAsync(user, dto.Password, false))
                .ReturnsAsync(SignInResult.Failed);

            Func<Task> act = async () => await _service.Login(dto);

            await act.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid credentials");
        }
    }
}
