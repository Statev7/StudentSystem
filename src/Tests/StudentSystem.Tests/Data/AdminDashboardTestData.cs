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

        public static IEnumerable<ApplicationUser> UsersForPromotionAndDemotion()
            => new List<ApplicationUser>()
            {
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
                },
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
                    Id = "ModeratorId",
                    UserRoles = new List<ApplicationUserRole>()
                    {
                        new ApplicationUserRole()
                        {
                            UserId = "ModeratorId",
                            Role = new ApplicationRole()
                            {
                                Id = "ModeratorRole",
                                Name = MODERATOR_ROLE,
                            }
                        }
                    }
                }
            };

        public static IEnumerable<ApplicationUser> UsersForBanAndUnban()
            => new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = "UserId",
                    IsDeleted = false,
                    UserRoles = new List<ApplicationUserRole>()
                    {
                        new ApplicationUserRole()
                        {
                            Role = new ApplicationRole()
                            {
                                Name = USER_ROLE,
                            }
                        }
                    }
                },
                new ApplicationUser()
                {
                    Id = "StudentId",
                    IsDeleted = false,
                    UserRoles = new List<ApplicationUserRole>()
                    {
                        new ApplicationUserRole()
                        {
                            Role = new ApplicationRole()
                            {
                                Name = STUDENT_ROLE,
                            }
                        }
                    }
                },
            };
    }
}
