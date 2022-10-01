namespace StudentSystem.Web.Infrastructure.Extensions
{
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Identity;

    using StudentSystem.Data.Models.StudentSystem;

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
    }
}
