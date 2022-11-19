namespace StudentSystem.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StudentSystem.Data.Models.StudentSystem;

    public class LessonsTestData
    {
        public static IEnumerable<Lesson> GetLessons()
            => Enumerable.Range(0, 12)
                .Select(x => new Lesson
                {
                    Id = x + 1,
                    Title = $"Test{x}",
                    CourseId = x % 2 == 0 ? 1 : 2,
                });

        public static IEnumerable<Course> CoursesIds()
            => Enumerable.Range(0, 10)
                .Select(x => new Course
                {
                    Id = x + 1,
                    StartDate = DateTime.UtcNow.AddDays(1),
                    EndDate = DateTime.UtcNow.AddDays(30),
                });

        public static IEnumerable<Course> CourseWithLessons()
            => Enumerable.Range(0, 10)
                .Select(x => new Course
                {
                    Id = x + 1,
                    StartDate = DateTime.UtcNow.AddDays(1),
                    EndDate = DateTime.UtcNow.AddDays(30),
                    Lessons = new List<Lesson>()
                    {
                        new Lesson
                        {
                            Id = x + 1,
                        }
                    }
                });
    }
}
