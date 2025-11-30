using BookHub.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookHub.Infrastructure.Seeding
{
        public static class SeedData
        {
            public static async Task SeedRolesAndAdmin(IServiceProvider services)
            {
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var config = services.GetRequiredService<IConfiguration>();

                var roles = config.GetSection("IdentityRoles").Get<string[]>();
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                var adminEmail = config["AdminSettings:Email"];
                var adminPassword = config["AdminSettings:Password"];
                var adminUserName = config["AdminSettings:UserName"];

                var admin = await userManager.FindByEmailAsync(adminEmail);

                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = adminUserName,
                        Email = adminEmail
                    };

                    var result = await userManager.CreateAsync(admin, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            }
        }
    }