namespace StudentSystem.Data.Seed.Implementation
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Data.Seed.Contracts;

    using static StudentSystem.Web.Common.GlobalConstants;

    public class UserSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (userManager.Users.Any())
            {
                return;
            }

            var admin = new ApplicationUser
            {
                Email = ADMIN_EMAIL,
                UserName = ADMIN_EMAIL
            };

            await userManager.CreateAsync(admin, ADMIN_PASSWORD);
            await userManager.AddToRoleAsync(admin, ADMIN_ROLE);
        }
    }
}
