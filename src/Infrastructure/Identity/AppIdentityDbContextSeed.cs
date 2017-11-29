namespace Infrastructure.Identity
{
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "prof.douglasbarcelos@gmail.com",
                Email = "prof.douglasbarcelos@gmail.com"
            };

            await userManager.CreateAsync(defaultUser, "1qaz@WSX");
        }
    }
}
