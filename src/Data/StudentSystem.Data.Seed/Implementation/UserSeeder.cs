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
                ImageURL = "https://preview.redd.it/cbc20oz17dg71.jpg?width=640&crop=smart&auto=webp&s=83e70dc910a8558c9955e9e6ee43cd0a98ccaa04",
                CreatedOn = DateTime.UtcNow,
            };

            var secondAdmin = new ApplicationUser
            {
                FirstName = "Inka",
                LastName = "Gadinka",
                Email = SHAMTKA_EMAIL,
                UserName = SHAMTKA_EMAIL,
                ImageURL = "https://animemotivation.com/wp-content/uploads/2022/07/levi-ackerman-aot-bad-ass.jpg",
                CreatedOn = DateTime.UtcNow,
            };

            await userManager.CreateAsync(admin, ADMIN_PASSWORD);
            await userManager.AddToRoleAsync(admin, ADMIN_ROLE);

            await userManager.CreateAsync(secondAdmin, SHAMTKA_PASSWORD);
            await userManager.AddToRoleAsync(secondAdmin, ADMIN_ROLE);
        }
    }
}
