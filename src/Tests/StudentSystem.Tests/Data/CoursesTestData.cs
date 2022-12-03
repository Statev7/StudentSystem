namespace StudentSystem.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StudentSystem.Data.Models.StudentSystem;

    public static class CoursesTestData
    {
        public static IEnumerable<Course> CoursesWithCategory()
            => Enumerable
                .Range(0, 10)
                .Select(c => new Course
                {
                    Id = c + 1,
                    CourseCategories = new List<CourseCategory>()
                    {
                        new CourseCategory
                        {
                            CourseId = c + 1,
                            Category = new Category()
                            {
                                Id = c + 1,
                            }
                        }
                    },
                    StartDate = DateTime.UtcNow.AddDays(1),
                });

        public static IEnumerable<Category> Categories()
            => Enumerable.Range(0, 10).Select(c => new Category { Id = c + 1 });
    }
}
