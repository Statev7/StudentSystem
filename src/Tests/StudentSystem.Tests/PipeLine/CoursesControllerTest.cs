namespace StudentSystem.Tests.PipeLine
{
    using System;
    using System.Linq;

    using MyTested.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Course.Models;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.Web.Areas.Trainings.Controllers;

    using Xunit;

    using static StudentSystem.Tests.Data.CoursesTestData;
    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Web.Common.NotificationsConstants;

    public class CoursesControllerTest
    {
        private const string ROUTE = "Trainings/Courses";

        public static readonly object[][] ValidParameters =
        {
            new object[]
            {
                "Course Name",
                "CourseDescription",
                "https://cdn.hackr.io/uploads/posts/large/1587557376h8DXhQA9Qk.png",
                new string[] { "1" },
                DateTime.UtcNow.AddDays(1).ToString(),
                DateTime.UtcNow.AddDays(10).ToString(),
            },
        };

        public static readonly object[][] InvalidParameters =
        {
            new object[]
            {
                "a", "b", "a", new string[]{"1"}, DateTime.UtcNow, DateTime.UtcNow,
            },
            new object[]
            {
                "Course Name",
                "CourseDescription",
                "https://cdn.hackr.io/uploads/posts/large/1587557376h8DXhQA9Qk.png",
                new string[] { "1" },
                DateTime.UtcNow.ToString(),
                DateTime.UtcNow.AddDays(1).ToString(),
            },
            new object[]
            {
                "Course Name",
                "CourseDescription",
                "https://cdn.hackr.io/uploads/posts/large/1587557376h8DXhQA9Qk.png",
                new string[] { "1" },
                DateTime.UtcNow.AddDays(3),
                DateTime.UtcNow.AddDays(2),
            }
        };

        public static readonly object[][] ValidParametesForUpdate =
        {
             new object[]
             {
                1,
                "Course Name",
                "CourseDescription",
                "https://cdn.hackr.io/uploads/posts/large/1587557376h8DXhQA9Qk.png",
                new string[] { "1" },
                DateTime.UtcNow.AddDays(1).ToString(),
                DateTime.UtcNow.AddDays(10).ToString(),
             },
        };

        public static readonly object[][] InvalidParametersForUpdate =
        {
            new object[]
            {
                1, "a", "b", "a", new string[]{"1"}, DateTime.UtcNow, DateTime.UtcNow,
            },
            new object[]
            {
                1,
                "Course Name",
                "CourseDescription",
                "https://cdn.hackr.io/uploads/posts/large/1587557376h8DXhQA9Qk.png",
                new string[] { "1" },
                DateTime.UtcNow.ToString(),
                DateTime.UtcNow.AddDays(1).ToString(),
            },
            new object[]
            {
                1,
                "Course Name",
                "CourseDescription",
                "https://cdn.hackr.io/uploads/posts/large/1587557376h8DXhQA9Qk.png",
                new string[] { "1" },
                DateTime.UtcNow.AddDays(3),
                DateTime.UtcNow.AddDays(2),
            }
        };

        public static readonly object[][] ValidModelStateWithInvalidCourseId =
        {
            new object[]
            {
                -1,
                "Course Name",
                "CourseDescription",
                "https://cdn.hackr.io/uploads/posts/large/1587557376h8DXhQA9Qk.png",
                new string[] { "1" },
                DateTime.UtcNow.AddDays(1).ToString(),
                DateTime.UtcNow.AddDays(10).ToString(),
            },
        };

        [Theory]
        [InlineData(new int[] { }, 1)]
        public void IndexShouldReturnViewWithCorrectData(int[] filters, int currentPage)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Index?filters={string.Join("&filters=", filters)}&currentPage={currentPage}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<CoursesController>(c => c.Index(filters, currentPage))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<AllCoursesViewModel>(model =>
                    {
                        Assert.Equal(6, model.Courses.Count());
                        Assert.Equal(10, model.Categories.Count());
                    }));

        [Theory]
        [InlineData(new int[] { 1 }, 1)]
        [InlineData(new int[] { 1, 2 }, 1)]
        [InlineData(new int[] { 1, 2, 3 }, 1)]
        public void IndexShouldFilterAndReturnCorrectData(int[] filters, int currentPage)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Index?filters={string.Join("&filters=", filters)}&currentPage={currentPage}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<CoursesController>(c => c.Index(filters, currentPage))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<AllCoursesViewModel>(model =>
                    {
                        Assert.Equal(filters.Length, model.Courses.Count());
                        Assert.Equal(10, model.Categories.Count());
                    }));

        [Fact]
        public void CreateShouldReturnViewWithCorrectModel()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Create")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<CoursesController>(c => c.Create())
                .Which()
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<CourseFormServiceModel>());

        [Theory, MemberData(nameof(ValidParameters))]
        public void PostCreateShouldRedirect(
            string name,
            string description,
            string imageUrl,
            string[] categoriesIds,
            string startDate,
            string endDate)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Create")
                    .WithFormFields(new
                    {
                        Name = name,
                        Description = description,
                        ImageUrl = imageUrl,
                        CategoriesIds = categoriesIds.FirstOrDefault(),
                        StartDate = startDate,
                        EndDate = endDate,
                    })
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<CoursesController>(controller => controller.Create(new CourseFormServiceModel
                {
                    Name = name,
                    Description = description,
                    ImageURL = imageUrl,
                    CategoriesIds = categoriesIds.Select(int.Parse).ToArray(),
                    StartDate = DateTime.Parse(startDate),
                    EndDate = DateTime.Parse(endDate),
                }))
                .Which(controller => controller
                    .WithData(Categories()))
                .ShouldReturn()
                .RedirectToAction("Index");

        [Theory, MemberData(nameof(InvalidParameters))]
        public void PostCreateShouldReturnViewIfModelStateInInvalid(
            string name,
            string description,
            string imageUrl,
            string[] categoriesIds,
            string startDate,
            string endDate)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Create")
                    .WithFormFields(new
                    {
                        Name = name,
                        Description = description,
                        ImageUrl = imageUrl,
                        CategoriesIds = categoriesIds.FirstOrDefault(),
                        StartDate = startDate,
                        EndDate = endDate,
                    })
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<CoursesController>(controller => controller.Create(new CourseFormServiceModel
                {
                    Name = name,
                    Description = description,
                    ImageURL = imageUrl,
                    CategoriesIds = categoriesIds.Select(int.Parse).ToArray(),
                    StartDate = DateTime.Parse(startDate),
                    EndDate = DateTime.Parse(endDate),
                }))
                .Which(controller => controller
                    .WithData(Categories()))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<CourseFormServiceModel>(model => model.Categories.Count == 10));

        [Theory, MemberData(nameof(ValidParameters))]
        public void PostCreateShouldSaveCourseInDb(
            string name,
            string description,
            string imageUrl,
            string[] categoriesIds,
            string startDate,
            string endDate)
             => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Create")
                    .WithFormFields(new
                    {
                        Name = name,
                        Description = description,
                        ImageUrl = imageUrl,
                        CategoriesIds = categoriesIds.FirstOrDefault(),
                        StartDate = startDate,
                        EndDate = endDate,
                    })
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<CoursesController>(controller => controller.Create(new CourseFormServiceModel
                {
                    Name = name,
                    Description = description,
                    ImageURL = imageUrl,
                    CategoriesIds = categoriesIds.Select(int.Parse).ToArray(),
                    StartDate = DateTime.Parse(startDate),
                    EndDate = DateTime.Parse(endDate),
                }))
                .Which(controller => controller
                    .WithData(Categories()))
                .ShouldHave()
                .ValidModelState()
                .Data(data => data
                    .WithSet<Course>(courses => courses
                        .Any(c =>
                            c.Name == name &&
                            c.Description == description &&
                            c.ImageURL == imageUrl &&
                            c.StartDate == DateTime.Parse(startDate) &&
                            c.EndDate == DateTime.Parse(endDate))))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(SUCCESS_NOTIFICATION));

        [Theory]
        [InlineData(1)]
        public void UpdateShouldReturnViewWithCorrectModel(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Update?id={id}")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<CoursesController>(c => c.Update(id))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldReturn()
                .View(result => result.WithModelOfType<CourseFormServiceModel>());

        [Theory]
        [InlineData(-1)]
        public void UpdateShouldRedirectIfCourseNotExist(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Update?id={id}")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<CoursesController>(c => c.Update(id))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(ERROR_NOTIFICATION))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Index");


        [Theory, MemberData(nameof(ValidParametesForUpdate))]
        public void PostUpdateShouldRedirect(
            int id,
            string name,
            string description,
            string imageUrl,
            string[] categoriesIds,
            string startDate,
            string endDate)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Update?id={id}")
                    .WithFormFields(new
                    {
                        Name = name,
                        Description = description,
                        ImageUrl = imageUrl,
                        CategoriesIds = categoriesIds.FirstOrDefault(),
                        StartDate = startDate,
                        EndDate = endDate,
                    })
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<CoursesController>(c => c.Update(id, new CourseFormServiceModel
                {
                    Name = name,
                    Description = description,
                    ImageURL = imageUrl,
                    CategoriesIds = categoriesIds.Select(int.Parse).ToArray(),
                    StartDate = DateTime.Parse(startDate),
                    EndDate = DateTime.Parse(endDate),
                }))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldReturn()
                .RedirectToAction("Index");

        [Theory,  MemberData(nameof(InvalidParametersForUpdate))]
        public void PostUpdateShouldReturnViewIfModelStateIsInvalid(
            int id,
            string name,
            string description,
            string imageUrl,
            string[] categoriesIds,
            string startDate,
            string endDate)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Update?id={id}")
                    .WithFormFields(new
                    {
                        Name = name,
                        Description = description,
                        ImageUrl = imageUrl,
                        CategoriesIds = categoriesIds.FirstOrDefault(),
                        StartDate = startDate,
                        EndDate = endDate,
                    })
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<CoursesController>(c => c.Update(id, new CourseFormServiceModel
                {
                    Name = name,
                    Description = description,
                    ImageURL = imageUrl,
                    CategoriesIds = categoriesIds.Select(int.Parse).ToArray(),
                    StartDate = DateTime.Parse(startDate),
                    EndDate = DateTime.Parse(endDate),
                }))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<CourseFormServiceModel>(model => model.Categories.Count == 10));

        [Theory, MemberData(nameof(ValidModelStateWithInvalidCourseId))]
        public void PostUpdateShouldRedirectIfCourseNotExist(
            int id,
            string name,
            string description,
            string imageUrl,
            string[] categoriesIds,
            string startDate,
            string endDate)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Update?id={id}")
                    .WithFormFields(new
                    {
                        Name = name,
                        Description = description,
                        ImageUrl = imageUrl,
                        CategoriesIds = categoriesIds.FirstOrDefault(),
                        StartDate = startDate,
                        EndDate = endDate,
                    })
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<CoursesController>(c => c.Update(id, new CourseFormServiceModel
                {
                    Name = name,
                    Description = description,
                    ImageURL = imageUrl,
                    CategoriesIds = categoriesIds.Select(int.Parse).ToArray(),
                    StartDate = DateTime.Parse(startDate),
                    EndDate = DateTime.Parse(endDate),
                }))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(ERROR_NOTIFICATION))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Index");

        [Theory, MemberData(nameof(ValidParametesForUpdate))]
        public void PostUpdateShouldUpdateCourseAndSaveToDb(
            int id,
            string name,
            string description,
            string imageUrl,
            string[] categoriesIds,
            string startDate,
            string endDate)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Update?id={id}")
                    .WithFormFields(new
                    {
                        Name = name,
                        Description = description,
                        ImageUrl = imageUrl,
                        CategoriesIds = categoriesIds.FirstOrDefault(),
                        StartDate = startDate,
                        EndDate = endDate,
                    })
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<CoursesController>(c => c.Update(id, new CourseFormServiceModel
                {
                    Name = name,
                    Description = description,
                    ImageURL = imageUrl,
                    CategoriesIds = categoriesIds.Select(int.Parse).ToArray(),
                    StartDate = DateTime.Parse(startDate),
                    EndDate = DateTime.Parse(endDate),
                }))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Course>(courses => courses
                        .Any(c => 
                            c.Name == name &&
                            c.Description == description &&
                            c.ImageURL == imageUrl &&
                            c.StartDate == DateTime.Parse(startDate) &&
                            c.EndDate == DateTime.Parse(endDate))))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(SUCCESS_NOTIFICATION));

        [Theory]
        [InlineData(1)]
        public void DetailsShouldReturnViewWithCorrectModel(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Details?id={id}")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<CoursesController>(c => c.Details(id))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<DetailCourseViewModel>());

        [Theory]
        [InlineData(-1)]
        public void DetailsShouldRedirectIfCourseNotExist(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Details?id={id}")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<CoursesController>(c => c.Details(id))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(ERROR_NOTIFICATION))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Index");

        [Theory]
        [InlineData(1)]
        public void DeleteShouldRedirect(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Delete?id={id}")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<CoursesController>(c => c.Delete(id))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldReturn()
                .RedirectToAction("Index");

        [Theory]
        [InlineData(-1)]
        public void DeleteShouldRetirectIfCourseNotExist(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Delete?id={id}")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<CoursesController>(c => c.Delete(id))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(ERROR_NOTIFICATION))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Index");

        [Theory]
        [InlineData(1)]
        public void DeleteShouldMarkCourseInDbAsDeleted(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Delete?id={id}")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<CoursesController>(c => c.Delete(id))
                .Which(controller => controller
                    .WithData(CoursesWithCategory()))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Course>(courses => courses
                        .Any(c =>
                            c.Id == id &&
                            c.IsDeleted)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(SUCCESS_NOTIFICATION));
    }
}
