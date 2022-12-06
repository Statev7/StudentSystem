namespace StudentSystem.Tests.Data
{
    using StudentSystem.Data.Models.StudentSystem;

    using System.Collections.Generic;
    using System.Linq;

    public static class ResourceTestData
    {
        public static IEnumerable<Lesson> Lessons()
            => Enumerable
                .Range(0, 10)
                .Select(x => new Lesson { Id = x + 1 });

        public static IEnumerable<Resource> ResourcesForUpdate()
            => Enumerable
                .Range(0, 10)
                .Select(x => new Resource
                {
                    Id = x + 1,
                    Name = "SomeResource",
                    Lesson = new Lesson
                    {
                        Id = x + 1,
                    }
                });

        public static Resource ResourceForDetails()
            => new Resource
            {
                Id = 1,
                Name = "SomeResource",
                Lesson = new Lesson
                {
                    Course = new Course
                    {
                        Id = 1,
                        UserCourses = new List<UserCourse>
                        {
                            new UserCourse()
                            {
                                CourseId = 1,
                                ApplicationUserId = "TestId",
                            }
                        }
                    }
                }
            };
    }     
}
