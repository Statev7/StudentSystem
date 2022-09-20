namespace StudentSystem.Web.Infrastructure.Extensions
{
    using System.Linq;
    using System.Security.Claims;

    using StudentSystem.Web.Data;

    public static class ClaimsPrincipalExtensions
    {
        public static bool IsUserInCourseAlready(
            this ClaimsPrincipal claimsPrincipal,
            StudentSystemDbContext dbContext,
            int courseId,
            string userId)
        {
            if (userId == null)
            {
                return false;
            }

            var user = dbContext
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
    }
}
