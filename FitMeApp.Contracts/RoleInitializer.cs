using FitMeApp.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitMeApp.Contracts
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string email = InitializedRoleData.GetAdminEmail();
            string password = InitializedRoleData.GetAdminPassword();

            if (await roleManager.FindByNameAsync(Roles.admin.ToString()) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.admin.ToString()));
            }
            if (await roleManager.FindByNameAsync(Roles.user.ToString()) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.user.ToString()));
            }
            if (await userManager.FindByEmailAsync(email) == null)
            {
                User admin = new User(){UserName = email, Email = email};
                IdentityResult result = await userManager.CreateAsync(admin);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.admin.ToString());
                }
            }
        }
    }
}
