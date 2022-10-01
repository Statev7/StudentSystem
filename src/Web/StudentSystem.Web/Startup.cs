namespace StudentSystem.Web
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Course;
    using StudentSystem.Services.Home;
    using StudentSystem.Services.Lesson;
    using StudentSystem.Services.Review;
    using StudentSystem.Web.Data;
    using StudentSystem.Web.Infrastructure.Extensions;

    using static StudentSystem.Data.Seed.Launcher;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StudentSystemDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StudentSystemDbContext>();

            services.AddControllersWithViews();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            this.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.MigrateDatabaseAsync().GetAwaiter().GetResult();
            SeedDataBase(app).GetAwaiter().GetResult();


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ILessonService, LessonService>();
            services.AddTransient<IReviewService, ReviewService>();
        } 
    }
}
