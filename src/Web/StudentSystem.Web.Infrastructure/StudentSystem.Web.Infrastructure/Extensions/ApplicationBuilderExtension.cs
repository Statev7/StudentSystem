namespace StudentSystem.Web.Infrastructure.Extensions
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Web.Data;

    public static class ApplicationBuilderExtension
    {
        public static async Task MigrateDatabaseAsync(this IApplicationBuilder application)
        {
            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<StudentSystemDbContext>();

                await dbContext.Database.MigrateAsync();
            }
        }
    }
}
