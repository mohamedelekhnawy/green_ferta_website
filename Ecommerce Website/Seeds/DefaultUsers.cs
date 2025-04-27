using Ecommerce_Website.Conest;

namespace Ecommerce_Website.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager) 
        {
            ApplicationUser admin = new()
            {
                FullName = "Admin User",
                UserName = "greenferta@gmail.com",
                Email = "greenferta@gmail.com",
                EmailConfirmed = true,
            };
            var user = await userManager.FindByNameAsync(admin.UserName);
            if (user is null)
            {
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }
        }
    }
}
