namespace StudentSystem.Data.Seed.Implementation
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Data.Seed.Contracts;

    using static StudentSystem.Web.Common.GlobalConstants;

    public class RoleSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceScope serviceScope)
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (roleManager.Roles.Any())
            {
                return;
            }

            var userRole = new IdentityRole
            {
                Name = USER_ROLE
            };

            var adminRole = new IdentityRole
            {
                Name = ADMIN_ROLE
            };

            await roleManager.CreateAsync(userRole);
            await roleManager.CreateAsync(adminRole);
        }
    }
}
