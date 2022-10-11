namespace StudentSystem.Data.Seed.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Data.Seed.Contracts;
    using StudentSystem.Web.Data;

    public class CourseSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceScope serviceScope)
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<StudentSystemDbContext>();

            if (dbContext.Courses.Any())
            {
                return;
            }

            var lessonsForCSharpOOPCourse = new List<Lesson>()
            {
                new Lesson
                {
                    Title = "Inheritance",
                    Content = "What is enheritance?",
                    Begining = DateTime.UtcNow.AddDays(1),
                    End = DateTime.UtcNow.AddDays(1).AddHours(2),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Encapsulation",
                    Content = "What is encapsulation",
                    Begining = DateTime.UtcNow.AddDays(3),
                    End = DateTime.UtcNow.AddDays(3).AddHours(2),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Abstraction",
                    Content = "What is abstraction",
                    Begining = DateTime.UtcNow.AddDays(7),
                    End = DateTime.UtcNow.AddDays(7).AddHours(2),
                    CreatedOn = DateTime.UtcNow,
                },
            };

            var lessonsForCSharpDataStructure = new List<Lesson>()
            {
                new Lesson
                {
                    Title = "Data Structures and Complexity",
                    Content = "What is data structures?",
                    Begining = DateTime.UtcNow.AddDays(1),
                    End = DateTime.UtcNow.AddDays(1).AddHours(2),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Linear Data Structures",
                    Content = "Linear Data Structures",
                    Begining = DateTime.UtcNow.AddDays(3),
                    End = DateTime.UtcNow.AddDays(3).AddHours(2),
                    CreatedOn = DateTime.UtcNow,
                },
                new Lesson
                {
                    Title = "Trees",
                    Content = "What is tree?",
                    Begining = DateTime.UtcNow.AddDays(7),
                    End = DateTime.UtcNow.AddDays(7).AddHours(2),
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
                    StartDate = DateTime.UtcNow.AddDays(1),
                    EndDate = DateTime.UtcNow.AddMonths(2),
                    Lessons = lessonsForCSharpOOPCourse,
                },
                new Course
                {
                    Name = "Data Structure",
                    Description = "This course will help you in better understanding of the basics of Data Structures",
                    ImageURL = "https://media.geeksforgeeks.org/wp-content/uploads/20220520181109/WhatisDataStructure1.jpg",
                    CreatedOn = DateTime.UtcNow,
                    StartDate = DateTime.UtcNow.AddDays(1),
                    EndDate = DateTime.UtcNow.AddMonths(2),
                    Lessons = lessonsForCSharpDataStructure,
                }
            };

            foreach (var course in courses)
            {
                await dbContext.Courses.AddAsync(course);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
