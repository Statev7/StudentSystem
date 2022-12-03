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

        [Theory]
        [InlineData(1)]
        public void IndexShouldReturnView(int currentPage)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Index?currentPage{currentPage}")
                    .WithUser(new[] {ADMIN_ROLE})
                    .WithAntiForgeryToken())
                .To<DashboardController>(c => c.Index(null, 1))
                .Which(controller => controller
                    .WithData(Users()))
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<UserPageViewModel>(model =>
                    {
                        Assert.Equal(10, model.Users.Count());
                    }));

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
