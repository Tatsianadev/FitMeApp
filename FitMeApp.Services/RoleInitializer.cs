using FitMeApp.Common;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FitMeApp.Services
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string email = InitializedRoleData.GetAdminEmail();
            string password = InitializedRoleData.GetAdminPassword();

            if (await roleManager.FindByNameAsync(RolesEnum.admin.ToString()) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RolesEnum.admin.ToString()));
            }
            if (await roleManager.FindByNameAsync(RolesEnum.user.ToString()) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RolesEnum.user.ToString()));
            }

            if (await userManager.GetUsersInRoleAsync(RolesEnum.admin.ToString()) == null)
            {
                User admin = new User() { UserName = email, Email = email };
                IdentityResult result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, RolesEnum.admin.ToString());
                }
            }
        }
    }
}
