namespace StudentSystem.Tests.Data
{
    using StudentSystem.Data.Models.StudentSystem;

    using System;
    using System.Collections.Generic;

    public static class HomeControllerData
    {
        public static ApplicationUser StudentWithCourse()
            => new ApplicationUser
            {
                Id = "TestId",
                UserCourses = new List<UserCourse>()
                {
                    new UserCourse
                    {
                        ApplicationUserId = "TestId",
                        Course= new Course
                        {
                            Id = 1,
                            Name = "CSharp",
                            EndDate = DateTime.UtcNow.AddDays(1),
                            Lessons = new List<Lesson>()
                            {
                                new Lesson()
                                {
                                    Id = 1,
                                    Title = "Title",
                                    End = DateTime.UtcNow.AddDays(1),
                                }
                            }
                        }
                    }
                }
            };
    }
}
