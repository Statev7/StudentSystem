namespace StudentSystem.Web.Infrastructure.Filters
{
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Web.Data;

    public class UserStatusFilter : IActionFilter
    {
        private readonly StudentSystemDbContext dbContext;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserStatusFilter(StudentSystemDbContext dbContext, SignInManager<ApplicationUser> signInManager)
        {
            this.dbContext = dbContext;
            this.signInManager = signInManager;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                var isUserBanned = this.dbContext.Users
                    .Where(u => u.Id == userId)
                    .Select(x => x.IsDeleted)
                    .FirstOrDefaultAsync().GetAwaiter().GetResult();

                if (isUserBanned)
                {
                    this.signInManager.SignOutAsync().GetAwaiter().GetResult();
                    context.HttpContext.Response.Redirect("/");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
