namespace StudentSystem.Data.Seed
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Data.Seed.Contracts;
    using StudentSystem.Data.Seed.Implementation;

    public static class Launcher
    {
        public static async Task SeedDataBase(IApplicationBuilder application)
        {
            ICollection<ISeeder> seeders = new List<ISeeder>()
            {
                new RoleSeeder(),
                new UserSeeder(),
            };

            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                foreach (var seeder in seeders)
                {
                    await seeder.SeedAsync(serviceScope);
                }
            }
        }
    }
}
