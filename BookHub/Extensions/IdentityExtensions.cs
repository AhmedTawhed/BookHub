using BookHub.Core.Entities;
using BookHub.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace BookHub.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddBookHubIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            })
            .AddEntityFrameworkStores<BookHubDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}