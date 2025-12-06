using BookHub.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BookHub.Infrastructure.Data.Seeding
{
    public static class IdentitySeed
    {
        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
        public static async Task Seed(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            await CreateRole(roleManager, "Admin");
            await CreateRole(roleManager, "User");

            var email = "admin@bookhub.com";
            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                };

                await userManager.CreateAsync(admin, "Admin@123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

    }
}