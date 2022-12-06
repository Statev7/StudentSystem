namespace StudentSystem.Data.Seed.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Data.Seed.Contracts;
    using StudentSystem.Web.Data;

    using static StudentSystem.Web.Common.GlobalConstants;

    public class CourseSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceScope serviceScope)
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<StudentSystemDbContext>();

            if (await dbContext.Courses.AnyAsync())
            {
                return;
            }

            var lessonsForCSharpOOPCourse = new List<Lesson>()
            {
                new Lesson
                {
                    Title = "Inheritance",
                    Content = "What is enheritance?",
                    Begining = DateTime.UtcNow.AddDays(5),
                    End = DateTime.UtcNow.AddDays(5).AddHours(4),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Encapsulation",
                    Content = "What is encapsulation",
                    Begining = DateTime.UtcNow.AddDays(8),
                    End = DateTime.UtcNow.AddDays(8).AddHours(4),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Abstraction",
                    Content = "What is abstraction",
                    Begining = DateTime.UtcNow.AddDays(12),
                    End = DateTime.UtcNow.AddDays(12).AddHours(4),
                    CreatedOn = DateTime.UtcNow,
                },
            };

            var lessonsForJavaAdvanced = new List<Lesson>()
            {
                new Lesson
                {
                    Title = "Stack and Queue",
                    Content = $"What is data stack? {Environment.NewLine}What is queue?",
                    Begining = DateTime.UtcNow.AddDays(5),
                    End = DateTime.UtcNow.AddDays(5).AddHours(3),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Multidimensional Arrays",
                    Content = "What is Multidimensional Array?",
                    Begining = DateTime.UtcNow.AddDays(8),
                    End = DateTime.UtcNow.AddDays(8).AddHours(3),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Sets and Maps",
                    Content = $"Sets {Environment.NewLine}Maps",
                    Begining = DateTime.UtcNow.AddDays(12),
                    End = DateTime.UtcNow.AddDays(12).AddHours(3),
                    CreatedOn = DateTime.UtcNow,
                },
            };

            var lessonsForReactCourse = new List<Lesson>()
            {
                new Lesson
                {
                    Title = "Intro to React",
                    Content = $"React Overview {Environment.NewLine}Intallation",
                    Begining = DateTime.UtcNow.AddDays(7),
                    End = DateTime.UtcNow.AddDays(7).AddHours(4),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Components",
                    Content = $"Components Overview {Environment.NewLine}Props{Environment.NewLine}State",
                    Begining = DateTime.UtcNow.AddDays(10),
                    End = DateTime.UtcNow.AddDays(10).AddHours(4),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Forms",
                    Content = $"Controlled Forms {Environment.NewLine}Validation",
                    Begining = DateTime.UtcNow.AddDays(14),
                    End = DateTime.UtcNow.AddDays(14).AddHours(4),
                    CreatedOn = DateTime.UtcNow,
                },
            };

            var lessonsForPythonAiCourse = new List<Lesson>()
            {
                new Lesson
                {
                    Title = "Introduction to Deep Learning",
                    Content = $"Linear and logistic regression {Environment.NewLine}Simple estimators",
                    Begining = DateTime.UtcNow.AddDays(7),
                    End = DateTime.UtcNow.AddDays(7).AddHours(4),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Image-Related Neural Networks",
                    Content = $"Statistical invariance {Environment.NewLine}Convolution networks",
                    Begining = DateTime.UtcNow.AddDays(10),
                    End = DateTime.UtcNow.AddDays(10).AddHours(4),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Neural Network Architectures",
                    Content = $"Residual networks {Environment.NewLine}Encoder-decoder: ENet",
                    Begining = DateTime.UtcNow.AddDays(14),
                    End = DateTime.UtcNow.AddDays(14).AddHours(4),
                    CreatedOn = DateTime.UtcNow,
                },
            };

            var courses = new List<Course>()
            {
                new Course
                {
                    Name = "C# OOP",
                    Description = "This course takes you, step by step, through the principles and practices of object-oriented programming (OOP). The course provides you with the firm foundation in OOP that you need to progress to intermediate-level C# courses.",
                    ImageURL = "https://techgeekbuzz.com/media/post_images/uploads/2021/09/C-Courses.jpg",
                    CreatedOn = DateTime.UtcNow,
                    StartDate = DateTime.UtcNow.AddDays(5),
                    EndDate = DateTime.UtcNow.AddMonths(2),
                    Lessons = lessonsForCSharpOOPCourse,
                    Reviews = new List<Review>(),
                },
                new Course
                {
                    Name = "Java Advanced",
                    Description = $"Discover and learn advanced aspects of Java programming, and some important Java-related concepts and technologies.{Environment.NewLine}This course can help you bridge the gap between the knowledge you have as a self-taught Java developer, junior developer or new IT graduate, and the knowledge that professional developers may have.",
                    ImageURL = "https://www.classcentral.com/report/wp-content/uploads/2022/05/Java-BCG-Banner.png",
                    CreatedOn = DateTime.UtcNow,
                    StartDate = DateTime.UtcNow.AddDays(6),
                    EndDate = DateTime.UtcNow.AddMonths(2),
                    Lessons = lessonsForJavaAdvanced,
                    Reviews = new List<Review>(),
                },
                new Course
                {
                    Name = "React",
                    Description = $"This course will cover basic and core concepts that you need to know to get up and running with ReactJS.{Environment.NewLine}React JS is a Javascript library for building user interfaces. It's flexible, fast, easy to learn and fun to work with.",
                    ImageURL = "https://149611589.v2.pressablecdn.com/wp-content/uploads/2016/09/reactjs.png",
                    CreatedOn = DateTime.UtcNow,
                    StartDate = DateTime.UtcNow.AddDays(7),
                    EndDate = DateTime.UtcNow.AddMonths(1).AddDays(7),
                    Lessons = lessonsForReactCourse,
                    Reviews = new List<Review>(),
                },
                new Course
                {
                    Name = "Python AI Machine Learning",
                    Description = $"Ready to explore machine learning and artificial intelligence in python? This course talks about fundamental Machine Learning algorithms, neural network and Deep Learning",
                    ImageURL = "https://i.ytimg.com/vi/7O60HOZRLng/maxresdefault.jpg",
                    CreatedOn = DateTime.UtcNow,
                    StartDate = DateTime.UtcNow.AddDays(7),
                    EndDate = DateTime.UtcNow.AddMonths(4).AddDays(7),
                    Lessons = lessonsForPythonAiCourse,
                    Reviews = new List<Review>(),
                }
            };

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == ADMIN_EMAIL);
            courses[0].Reviews.Add(new Review
            {
                User = user,
                Content = "Amazing course!",
                CreatedOn = DateTime.UtcNow,
            });

            courses[1].Reviews.Add(new Review
            {
                User = user,
                Content = "Very good course!",
                CreatedOn = DateTime.UtcNow.AddDays(2),
            });

            courses[2].Reviews.Add(new Review
            {
                User = user,
                Content = "I recommend!",
                CreatedOn = DateTime.UtcNow.AddDays(1),
            });

            courses[0].CourseCategories.Add(new CourseCategory
            {
                Course = courses[0],
                Category = new Category
                {
                    Name = "C#",
                    CreatedOn = DateTime.UtcNow,
                },
                CreatedOn = DateTime.UtcNow,
            });

            courses[1].CourseCategories.Add(new CourseCategory
            {
                Course = courses[1],
                Category = new Category
                {
                    Name = "Java",
                    CreatedOn = DateTime.UtcNow,
                },
                CreatedOn = DateTime.UtcNow,
            });

            courses[2].CourseCategories.Add(new CourseCategory
            {
                Course = courses[2],
                Category = new Category
                {
                    Name = "JavaScript",
                    CreatedOn = DateTime.UtcNow,
                },
                CreatedOn = DateTime.UtcNow,
            });

            courses[3].CourseCategories.Add(new CourseCategory
            {
                Course = courses[3],
                Category = new Category
                {
                    Name = "Python",
                    CreatedOn = DateTime.UtcNow,
                },
                CreatedOn = DateTime.UtcNow,
            });

            foreach (var course in courses)
            {
                await dbContext.Courses.AddAsync(course);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
