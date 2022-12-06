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
        [InlineData(null, 1)]
        public void IndexShouldReturnView(string search, int currentPage)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Index?search={search}&currentPage={currentPage}")
                    .WithUser(new[] {ADMIN_ROLE})
                    .WithAntiForgeryToken())
                .To<DashboardController>(c => c.Index(search, currentPage))
                .Which(controller => controller
                    .WithData(Users()))
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<UserPageViewModel>(model =>
                    {
                        Assert.Equal(5, model.Users.Count());
                    }));

        // Promotion tests

        [Theory]
        [InlineData("TestId")]
        public void PromotionShouldRedirectIfUserIsBanned(string id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Promotion")
                    .WithFormField("id", id)
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<DashboardController>(c => c.Promotion(id))
                .Which()
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(WARNING_NOTIFICATION))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Index");

        //Ban

        [Theory]
        [InlineData("UserId")]
        public void BanShoulMarkUserAsBanned(string id)
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
                    .WithData(new ApplicationUser { Id = "UserId", IsDeleted = false }))
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

        //Unban

        [Theory]
        [InlineData("UserId")]
        public void UnbanShouldMarkUserAsUnbanned(string id)
        => MyMvc
            .Pipeline()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation($"{ROUTE}/Unban")
                .WithFormField("id", id)
                .WithUser(new[] { ADMIN_ROLE })
                .WithAntiForgeryToken())
            .To<DashboardController>(c => c.Unban(id))
            .Which(controller => controller
                .WithData(new ApplicationUser { Id = "UserId", IsDeleted = true }))
            .ShouldHave()
            .Data(data => data
                .WithSet<ApplicationUser>(users => users
                    .Any(u =>
                        u.Id == id &&
                        !u.IsDeleted)))
            .TempData(tempData => tempData
                .ContainingEntryWithKey(SUCCESS_NOTIFICATION))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction("Index");
    }
}
