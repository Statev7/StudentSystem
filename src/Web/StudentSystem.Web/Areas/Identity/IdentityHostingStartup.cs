using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(StudentSystem.Web.Areas.Identity.IdentityHostingStartup))]
namespace StudentSystem.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}