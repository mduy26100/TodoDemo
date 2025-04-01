using Domain.Entities.ApplicationIdentity;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataSeeds
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if(!await roleManager.RoleExistsAsync(RoleStatus.Admin.ToString()))
            {
                await roleManager.CreateAsync(new ApplicationRole(RoleStatus.Admin.ToString(), "Administrator Role"));
            }

            if(!await roleManager.RoleExistsAsync(RoleStatus.User.ToString()))
            {
                await roleManager.CreateAsync(new ApplicationRole(RoleStatus.User.ToString(), "Defautl User Role"));
            }

            var emailAdmin = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(emailAdmin);

            if(adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    FullName = "Admin User",
                    UserName = emailAdmin,
                    Email = emailAdmin
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@123");

                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, RoleStatus.Admin.ToString());
                }
            }
        }
    }
}
