using Ecommerce_Website.Conest;

namespace Ecommerce_Website.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedRolesAsync (RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));
                await roleManager.CreateAsync(new IdentityRole(AppRoles.User));
            }
        }
    }
}
