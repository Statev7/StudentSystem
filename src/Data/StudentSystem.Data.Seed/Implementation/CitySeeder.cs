namespace StudentSystem.Data.Seed.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Data.Seed.Contracts;
    using StudentSystem.Web.Data;

    public class CitySeeder : ISeeder
    {
        public async Task SeedAsync(IServiceScope serviceScope)
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<StudentSystemDbContext>();

            if (await dbContext.Cities.AnyAsync())
            {
                return;
            }

            var cities = new List<City>()
            {
                new City{Name = "Sofia", CreatedOn = DateTime.UtcNow},
                new City{Name = "Plovdiv", CreatedOn = DateTime.UtcNow},
                new City{Name = "Varna", CreatedOn = DateTime.UtcNow},
                new City{Name = "Burgas", CreatedOn = DateTime.UtcNow},
                new City{Name = "Ruse", CreatedOn = DateTime.UtcNow},
                new City{Name = "Stara Zagora", CreatedOn = DateTime.UtcNow},
                new City{Name = "Pleven", CreatedOn = DateTime.UtcNow},
                new City{Name = "Sliven", CreatedOn = DateTime.UtcNow},
                new City{Name = "Dobrich", CreatedOn = DateTime.UtcNow},
                new City{Name = "Shumen", CreatedOn = DateTime.UtcNow},
                new City{Name = "Pernik", CreatedOn = DateTime.UtcNow},
                new City{Name = "Haskovo", CreatedOn = DateTime.UtcNow},
                new City{Name = "Yambol", CreatedOn = DateTime.UtcNow},
                new City{Name = "Pazardzhik", CreatedOn = DateTime.UtcNow},
                new City{Name = "Blagoevgrad", CreatedOn = DateTime.UtcNow},
                new City{Name = "Veliko Tarnovo", CreatedOn = DateTime.UtcNow},
                new City{Name = "Vratsa", CreatedOn = DateTime.UtcNow},
                new City{Name = "Gabrovo", CreatedOn = DateTime.UtcNow},
                new City{Name = "Kyustendil", CreatedOn = DateTime.UtcNow},
                new City{Name = "Vidin", CreatedOn = DateTime.UtcNow},
                new City{Name = "Montana", CreatedOn = DateTime.UtcNow},
                new City{Name = "Targovishte", CreatedOn = DateTime.UtcNow},
                new City{Name = "Lovech", CreatedOn = DateTime.UtcNow},
                new City{Name = "Silistra", CreatedOn = DateTime.UtcNow},
                new City{Name = "Razgrad", CreatedOn = DateTime.UtcNow},
                new City{Name = "Smolyan", CreatedOn = DateTime.UtcNow},
                new City{Name = "Kardzhali", CreatedOn = DateTime.UtcNow},
            };

            await dbContext.Cities.AddRangeAsync(cities);
            await dbContext.SaveChangesAsync();
        }
    }
}
