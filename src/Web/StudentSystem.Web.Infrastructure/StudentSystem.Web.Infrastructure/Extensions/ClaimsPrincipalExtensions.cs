namespace StudentSystem.Web.Infrastructure.Extensions
{
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Identity;

    using StudentSystem.Data.Models.StudentSystem;

    using static StudentSystem.Web.Common.GlobalConstants;

    public static class ClaimsPrincipalExtensions
    {
        public static bool IsUserInCourse(
            this ClaimsPrincipal claimsPrincipal,
            UserManager<ApplicationUser> userManager,
            int courseId,
            string userId)
        {
            if (userId == null)
            {
                return false;
            }

            var user = userManager
                .Users
                .Select(u => new
                {
                    UserId = u.Id,
                    Courses = u.UserCourses.Select(us => new
                    {
                        CourseId = us.CourseId,
                    })
                })
                .SingleOrDefault(x => x.UserId == userId);

            var isInCourse = user
                    .Courses
                    .Any(x => x.CourseId == courseId);

            return isInCourse;
        }

        public static string GetId(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        public static bool IsAdministrator(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.IsInRole(ADMIN_ROLE);
    }
}
