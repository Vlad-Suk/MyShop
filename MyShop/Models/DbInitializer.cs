using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Models
{
    public class DbInitializer
    {
        public static async Task Init (IServiceProvider provider)
        {
            var ctx = provider.GetRequiredService<AppDbContext>();

            ctx.Database.EnsureCreated();
            await EnsureAdminCreated(provider);

        }
        protected static async Task EnsureAdminCreated(IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var configuration = provider.GetRequiredService<IConfiguration>();

            string[] roleNames = { Roles.Admin, Roles.Customer };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if(!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminData = configuration.GetSection("Admin");
            var admin = await userManager.FindByEmailAsync(adminData["Email"]);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    Email = adminData["Email"],
                    FullName = adminData["FullName"],
                    UserName = adminData["UserName"],
                };
            }
            var createAdmin = await userManager.CreateAsync(admin, adminData["Password"]);
            if(createAdmin.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }
    }
}
