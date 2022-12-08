namespace StudentSystem.Tests.PipeLine
{
    using System;

    using System.Linq;

    using MyTested.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Lesson.Models;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.Web.Controllers;

    using Xunit;

    using static StudentSystem.Tests.Data.LessonsTestData;
    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Web.Common.NotificationsConstants;

    public class LessonControllerUpdateActionTest
    {
        public const string ROUTE = "/Trainings/Lessons";

        public readonly static object[][] ValidParametersToUpdate =
        {
            new object[]
            {
                1,
                "New title",
                "New lesson content",
                DateTime.UtcNow.AddDays(1).ToString(),
                DateTime.UtcNow.AddDays(1).AddHours(1).ToString(),
                1,
            },
            new object[]
            {
                1,
                "New title",
                "New lesson content",
                DateTime.UtcNow.AddDays(2).ToString(),
                DateTime.UtcNow.AddDays(2).AddHours(1).ToString(),
                2,
            }
        };

        public readonly static object[][] ValidParametersToCreate =
        {
            new object[]
            {
                "New title",
                "New lesson content",
                DateTime.UtcNow.AddDays(1).ToString(),
                DateTime.UtcNow.AddDays(1).AddHours(1).ToString(),
                1,
            },
            new object[]
            {
                "New title",
                "New lesson content",
                DateTime.UtcNow.AddDays(2).ToString(),
                DateTime.UtcNow.AddDays(2).AddHours(1).ToString(),
                2,
            }
        };

        public readonly static object[][] ParametersWithInvalidLessonId =
        {
            new object[]
            {
                0,
                "Some title",
                "Some content",
                DateTime.UtcNow.AddDays(1).ToString(),
                DateTime.UtcNow.AddDays(1).ToString(),
                1
            },
            new object[]
            {
                int.MinValue,
                "Some title",
                "Some content",
                DateTime.UtcNow.AddDays(1).ToString(),
                DateTime.UtcNow.AddDays(1).ToString(),
                1
            }

        };

        public readonly static object[][] InvalidModelStateParametersForUpdate =
        {
            new object[]
            {
                1,
                "I",
                "I",
                DateTime.UtcNow.AddMinutes(-1),
                DateTime.UtcNow.AddDays(1),
                0,
            },
            new object[]
            {
                1,
                "I",
                "I",
                DateTime.UtcNow.AddDays(2),
                DateTime.UtcNow.AddDays(1),
                0,
            }
        };

        public readonly static object[][] InvalidModelStateParametersForCreate =
        {
            new object[]
            {
                "Test Title",
                "Test Content",
                DateTime.UtcNow.AddDays(-1),
                DateTime.UtcNow.AddDays(-1),
                1,
            },
            new object[]
            {
                "Test Title",
                "Test Content",
                DateTime.UtcNow.AddDays(-1),
                DateTime.UtcNow.AddDays(-1),
                1,
            },
            new object[]
            {
                null,
                null,
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(1).AddHours(1),
                1,
            }
        };


        [Theory]
        [InlineData(new int[] { }, 1)]
        [InlineData(new int[] { }, 2)]
        [InlineData(new int[] { 1 }, 1)]
        [InlineData(new int[] { 2 }, 1)]
        [InlineData(new int[] { 1, 2 }, 1)]
        [InlineData(new int[] { 1, 2 }, 2)]
        public void IndexShoudReturnCorrectData(int[] filters, int currentPage)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation(
                        $"{ROUTE}/Index?filters={string.Join("&filters=", filters)}&currentPage={currentPage}")
                    .WithUser(TestUser.Username, new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
            .To<LessonsController>(c => c.Index(filters, currentPage))
            .Which(controller => controller
                .WithData(GetLessons()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View(result => result.WithModelOfType<PageLessonViewModel>()
                .Passing(model =>
                {
                    var isEquel = model.Filters.SequenceEqual(filters);

                    Assert.True(isEquel);
                    Assert.Equal(6, model.Lessons.Count());
                }));

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(12)]
        public void UpdateShouldReturnView(int id)
            =>
                MyMvc
                    .Pipeline()
                    .ShouldMap(request => request
                        .WithLocation($"{ROUTE}/Update/{id}")
                        .WithUser(new[] { ADMIN_ROLE })
                        .WithAntiForgeryToken())
                    .To<LessonsController>(c => c.Update(id))
                    .Which(controller => controller
                        .WithData(GetLessons()))
                    .ShouldReturn()
                    .View(result => result.WithModelOfType<LessonFormServiceModel>());

        [Fact]
        public void CreateShouldReturnCorrectView()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Create")
                    .WithUser(TestUser.Username, new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<LessonsController>(c => c.Create())
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(result => result.WithModelOfType<LessonFormServiceModel>());

        [Theory, MemberData(nameof(ValidParametersToCreate))]
        public void PostCreateShouldAddNewLessonToDb(
            string title,
            string content,
            string begining,
            string end,
            int courseId)
            =>
                MyMvc
                    .Pipeline()
                    .ShouldMap(request => request
                        .WithLocation($"{ROUTE}/Create")
                        .WithMethod(HttpMethod.Post)
                        .WithFormFields(new
                        {
                            Title = title,
                            Content = content,
                            Begining = begining,
                            End = end,
                            CourseId = courseId.ToString()
                        })
                         .WithUser(TestUser.Username, new[] { ADMIN_ROLE })
                         .WithAntiForgeryToken())
                         .To<LessonsController>(c => c.Create(new LessonFormServiceModel
                         {
                             Title = title,
                             Content = content,
                             Begining = DateTime.Parse(begining),
                             End = DateTime.Parse(end),
                             CourseId = courseId
                         }))
                        .Which(controller => controller
                            .WithData(CoursesIds()))
                        .ShouldHave()
                    .ValidModelState()
                    .Data(data => data
                        .WithSet<Lesson>(l => l
                            .Any(x =>
                                x.Title == title &&
                                x.Content == content &&
                                x.Begining == DateTime.Parse(begining) &&
                                x.End == DateTime.Parse(end) &&
                                x.CourseId == courseId)))
                    .TempData(tempData => tempData
                        .ContainingEntryWithKey(SUCCESS_NOTIFICATION))
                    .AndAlso()
                    .ShouldReturn()
                    .RedirectToAction("Index");

        [Theory, MemberData(nameof(InvalidModelStateParametersForCreate))]
        public void PostCreateShouldReturnViewIfModelStateIsInvalid(
            string title,
            string content,
            string begining,
            string end,
            int courseId)
            =>
                 MyMvc
                    .Pipeline()
                    .ShouldMap(request => request
                        .WithLocation($"{ROUTE}/Create")
                        .WithMethod(HttpMethod.Post)
                        .WithFormFields(new
                        {
                            Title = title,
                            Content = content,
                            Begining = begining,
                            End = end,
                            CourseId = courseId.ToString()

                        })
                         .WithUser(TestUser.Username, new[] { ADMIN_ROLE })
                         .WithAntiForgeryToken())
                         .To<LessonsController>(c => c.Create(new LessonFormServiceModel
                         {
                             Title = title,
                             Content = content,
                             Begining = DateTime.Parse(begining),
                             End = DateTime.Parse(end),
                             CourseId = courseId
                         }))
                        .Which(controller => controller
                            .WithData(CoursesIds()))
                        .ShouldHave()
                    .InvalidModelState()
                    .AndAlso()
                    .ShouldReturn()
                    .View(result => result.WithModelOfType<LessonFormServiceModel>()
                        .Passing(model => model.Courses.Count() == 10));

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public void DeleteShouldDeleteAEntityFromDb(int id)
            =>
                MyMvc
                    .Pipeline()
                    .ShouldMap(request => request
                        .WithLocation($"{ROUTE}/Delete/{id}")
                        .WithUser(new[] { ADMIN_ROLE })
                        .WithAntiForgeryToken())
                    .To<LessonsController>(c => c.Delete(id))
                    .Which(controller => controller
                        .WithData(GetLessons()))
                    .ShouldHave()
                    .Data(data => data
                        .WithSet<Lesson>(lessons => lessons
                            .Any(x => x.Id == id == false)))
                    .AndAlso()
                    .ShouldReturn()
                    .RedirectToAction("Index");
    }
}
