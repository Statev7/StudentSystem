namespace StudentSystem.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentSystem.Data.Models.StudentSystem;

    using static StudentSystem.Web.Common.GlobalConstants;

    public static class AdminDashboardTestData
    {
        public static IEnumerable<ApplicationUser> Users()
            => Enumerable.Range(0, 10)
                .Select(x => new ApplicationUser()
                {
                    UserCourses = new List<UserCourse>()
                    {
                        new UserCourse()
                        {
                            Course = new Course()
                            {

                            }
                        }
                    }
                });

        public static IEnumerable<Course> Courses()
            => Enumerable.Range(0, 10)
                .Select(x => new Course
                {
                    Id = x + 1,
                });

        public static IEnumerable<ApplicationUser> UsersForPromote()
            => new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = "StudentId",
                    UserRoles = new List<ApplicationUserRole>()
                    {
                        new ApplicationUserRole()
                        {
                            UserId = "StudentId",
                            Role = new ApplicationRole()
                            {
                                Id = "StudentRole",
                                Name = STUDENT_ROLE,
                            }
                        }
                    }

                },
                new ApplicationUser()
                {
                    Id = "UserId",
                    UserRoles = new List<ApplicationUserRole>()
                    {
                        new ApplicationUserRole()
                        {
                            UserId = "UserId",
                            Role = new ApplicationRole()
                            {
                                Id = "UserRole",
                                Name = USER_ROLE,
                            }
                        }
                    }
                }
            };

    }
}
