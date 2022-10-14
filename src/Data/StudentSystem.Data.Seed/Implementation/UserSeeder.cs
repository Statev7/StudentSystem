namespace StudentSystem.Data.Seed.Implementation
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Data.Seed.Contracts;

    using static StudentSystem.Web.Common.GlobalConstants;

    public class UserSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (await userManager.Users.AnyAsync())
            {
                return;
            }

            var admin = new ApplicationUser
            {
                FirstName = ADMIN_FIRST_NAME,
                LastName = ADMIN_LAST_NAME,
                Email = ADMIN_EMAIL,
                UserName = ADMIN_EMAIL,
                ImageURL = "https://i.pinimg.com/736x/33/32/6d/33326dcddbf15c56d631e374b62338dc.jpg",
                CreatedOn = DateTime.UtcNow,
            };

            await userManager.CreateAsync(admin, ADMIN_PASSWORD);
            await userManager.AddToRoleAsync(admin, ADMIN_ROLE);
        }
    }
}
