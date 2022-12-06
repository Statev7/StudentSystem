namespace StudentSystem.Tests.PipeLine
{
    using MyTested.AspNetCore.Mvc;

    using StudentSystem.ViewModels.Home;
    using StudentSystem.Web.Controllers;
    using StudentSystem.Web.Models;

    using System.Linq;

    using Xunit;

    using static StudentSystem.Tests.Data.HomeControllerData;
    using static StudentSystem.Web.Common.GlobalConstants;

    public class HomeControllerTest
    {
        public const string ROUTE = "Home";

        [Fact]
        public void IndexShouldReturnViewWithCorrectDataWhenUserIsNotStudentOrAdmin()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}"))
                .To<HomeController>(c => c.Index())
                .Which()
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<HomeViewModel>());

        [Fact]
        public void IndexShouldReturnViewWithCorrectDataWhenUserIsStudent()
        => MyMvc
            .Pipeline()
            .ShouldMap(request => request
                .WithLocation($"{ROUTE}")
                .WithUser(TestUser.Identifier, TestUser.Username, STUDENT_ROLE))
            .To<HomeController>(c => c.Index())
            .Which(controller => controller
                .WithData(StudentWithCourse()))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<HomeViewModel>(model =>
                {
                    Assert.Equal(1, model.StudentResources.Courses.Count());
                    Assert.Equal(1, model.StudentResources.Lessons.Count());
                }));

        [Fact]
        public void ErrorShouldReturnView()
            => MyMvc
            .Pipeline()
            .ShouldMap(request => request
                .WithLocation($"{ROUTE}/Error"))
            .To<HomeController>(c => c.Error())
            .Which()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ErrorViewModel>());
    }
}
