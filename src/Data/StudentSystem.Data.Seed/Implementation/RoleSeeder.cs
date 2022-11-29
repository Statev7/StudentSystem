namespace StudentSystem.Data.Seed.Implementation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Data.Seed.Contracts;

    using static StudentSystem.Web.Common.GlobalConstants;

    public class RoleSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceScope serviceScope)
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if (await roleManager.Roles.AnyAsync())
            {
                return;
            }

            var roles = new List<ApplicationRole>()
            {
                new ApplicationRole(USER_ROLE),
                new ApplicationRole(STUDENT_ROLE),
                new ApplicationRole(ADMIN_ROLE),
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }
    }
}
