namespace StudentSystem.Tests.Data
{
    using System.Collections.Generic;

    using StudentSystem.Data.Models.StudentSystem;

    public static class CoursesReviewsTestData
    {
        public static ApplicationUser GetUserWithCourse()
            => new ApplicationUser()
            {
                Id = "TestId",
                UserCourses = new List<UserCourse>()
                {
                    new UserCourse()
                    {
                        ApplicationUserId = "TestId",
                        Course = new Course()
                        {
                            Id = 1,
                        }
                    }
                }
            };

        public static Review ReviewWithUser()
            => new Review() { Id = 1, UserId = "TestId", Content = "Content" };
    }
}
