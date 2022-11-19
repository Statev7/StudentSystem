namespace StudentSystem.Tests.Controllers
{
    using MyTested.AspNetCore.Mvc;

    using StudentSystem.Web.Controllers;

    using Xunit;

    using static StudentSystem.Tests.Data.LessonsTestData;
    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Web.Common.NotificationsConstants;

    public class LessonControllerTests
    {
        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void UpdateShouldRedirectIfLessonIdNotExist(int id)
            =>
                MyController<LessonsController>
                    .Instance(controller => controller
                        .WithUser(new[] { ADMIN_ROLE })
                        .WithData(GetLessons()))
                    .Calling(c => c.Update(id))
                    .ShouldHave()
                    .TempData(tempData => tempData
                        .ContainingEntryWithKey(ERROR_NOTIFICATION))
                    .AndAlso()
                    .ShouldReturn()
                    .RedirectToAction("Index");

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public void DeleteShouldRedirectIfLessonIdNotExist(int id)
            =>
                MyController<LessonsController>
                    .Instance(controller => controller
                        .WithUser(new[] { ADMIN_ROLE })
                        .WithData(GetLessons()))
                    .Calling(c => c.Delete(id))
                    .ShouldHave()
                    .TempData(tempData => tempData
                        .ContainingEntryWithKey(ERROR_NOTIFICATION))
                    .AndAlso()
                    .ShouldReturn()
                    .RedirectToAction("Index");
    }
}
