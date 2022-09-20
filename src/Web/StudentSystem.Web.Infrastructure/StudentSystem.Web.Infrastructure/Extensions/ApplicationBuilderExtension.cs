namespace StudentSystem.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Web.Data;

    using System.Threading.Tasks;

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
