namespace StudentSystem.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentSystem.Data.Models.StudentSystem;

    public static class ExportDashboardTestData
    {
        public static IEnumerable<ApplicationUser> CoursesAndCities()
            => Enumerable
                .Range(0, 10)
                .Select(u => new ApplicationUser()
                {
                    Id = $"TestId{u}",
                    City = new City { Id = u + 1 },
                    UserCourses = new List<UserCourse>()
                    {
                        new UserCourse()
                        {
                            ApplicationUserId = $"TestId{u}",
                            Course = new Course { Id = u + 1 }
                        }
                    }
                });
    }
}
