namespace StudentSystem.Tests.PipeLine
{
    using System.Linq;

    using MyTested.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.ViewModels.User;
    using StudentSystem.Web.Areas.Administrator.Controllers;

    using Xunit;

    using static StudentSystem.Tests.Data.AdminDashboardTestData;
    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Web.Common.NotificationsConstants;

    public class DashboardControllerTest
    {
        private const string ROUTE = "Administrator/Dashboard";

        [Fact]
        public void IndexShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Index")
                    .WithUser(new[] {ADMIN_ROLE})
                    .WithAntiForgeryToken())
                .To<DashboardController>(c => c.Index())
                .Which(controller => controller
                    .WithData(Users()))
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<UsersCoursesViewModel>(model =>
                    {
                        Assert.Equal(10, model.Users.Count());
                        Assert.Equal(10, model.Courses.Count());
                    }));

        [Theory]
        [InlineData(1)]
        public void ExportShouldReturnFile(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Export?courseId={id}")
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<DashboardController>(c => c.Export(id))
                .Which(controller => controller
                    .WithData(Courses()))
                .ShouldReturn()
                .File();

        [Theory]
        [InlineData("UserId")]
        [InlineData("StudentId")]
        public void PromotionShouldPromoteUser(string id)
            => MyMvc
                .Pipeline()
                .ShouldMap(requst => requst
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Promotion")
                    .WithFormField("id", id)
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<DashboardController>(c => c.Promotion(id))
                .Which(controller => controller
                    .WithData(UsersForPromotionAndDemotion()))
                .ShouldHave()
                .Data(data => data
                    .WithSet<ApplicationUser>(users => users
                        .Any(u =>
                            u.Id == id &&
                            u.UserRoles.Count == 1 &&
                            u.UserRoles
                                .Any(ur => ur.Role.Name == STUDENT_ROLE || ur.Role.Name == MODERATOR_ROLE))))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(SUCCESS_NOTIFICATION))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Index");

        [Theory]
        [InlineData("StudentId")]
        [InlineData("ModeratorId")]
        public void DemoteShouldDemoteUser(string id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Demotion")
                    .WithFormField("Id", id)
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<DashboardController>(c => c.Demotion(id))
                .Which(controller => controller
                    .WithData(UsersForPromotionAndDemotion())
                .ShouldHave()
                .Data(data => data
                    .WithSet<ApplicationUser>(users => users
                        .Any(u =>
                            u.Id == id &&
                            u.UserRoles.Count == 1 &&
                            u.UserRoles
                                .Any(ur => ur.Role.Name == USER_ROLE || ur.Role.Name == STUDENT_ROLE))))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(SUCCESS_NOTIFICATION))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Index"));

        [Theory]
        [InlineData("UserId")]
        [InlineData("StudentId")]
        public void BanShouldBanUser(string id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/BanUser")
                    .WithFormField("id", id)
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<DashboardController>(c => c.BanUser(id))
                .Which(controller => controller
                    .WithData(UsersForBanAndUnban()))
                .ShouldHave()
                .Data(data => data
                    .WithSet<ApplicationUser>(users => users
                        .Any(u =>
                            u.Id == id &&
                            u.IsDeleted)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(SUCCESS_NOTIFICATION))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Index");
    }
}
