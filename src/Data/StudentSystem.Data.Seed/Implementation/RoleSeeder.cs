namespace StudentSystem.Data.Seed.Implementation
{
    using System.Collections.Generic;
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

            var roles = new List<IdentityRole>()
            {
                new IdentityRole(USER_ROLE),
                new IdentityRole(STUDENT_ROLE),
                new IdentityRole(MODERATOR_ROLE),
                new IdentityRole(ADMIN_ROLE),
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }
    }
}
