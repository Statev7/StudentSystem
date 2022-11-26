namespace StudentSystem.Tests.PipeLine
{
    using System.Linq;

    using MyTested.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Review.Models;
    using StudentSystem.Web.Areas.Trainings.Courses.Controllers;

    using Xunit;

    using static StudentSystem.Tests.Data.CoursesReviewsTestData;
    using static StudentSystem.Web.Common.NotificationsConstants;

    public class CourseReviewsControllerTest
    {
        private const string ROUTE = "Trainings/CourseReviews";

        [Theory]
        [InlineData(1, "Content")]
        public void CreateShouldRedirect(int courseId, string content)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Create?courseId={courseId}&content={content}")
                    .WithUser(TestUser.Identifier, TestUser.Username)
                    .WithAntiForgeryToken())
                .To<CourseReviewsController>(c => c.Create(courseId, content))
                .Which(controller => controller
                    .WithData(GetUserWithCourse()))
                .ShouldReturn()
                .RedirectToAction("Details", "Courses", new { Id = courseId });

        [Theory]
        [InlineData(1, null)]
        public void CreateShouldReturnViewIfContentIsEmpty(int courseId, string content)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Create?courseId={courseId}&content={content}")
                    .WithUser(TestUser.Identifier, TestUser.Username)
                    .WithAntiForgeryToken())
               .To<CourseReviewsController>(c => c.Create(courseId, content))
               .Which()
               .ShouldReturn()
               .View("Details");

        [Theory]
        [InlineData(2, "Content")]
        public void CreateShouldRedirectIfCourseNotExist(int courseId, string content)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Create?courseId={courseId}&content={content}")
                    .WithUser(TestUser.Identifier, TestUser.Username)
                    .WithAntiForgeryToken())
                .To<CourseReviewsController>(c => c.Create(courseId, content))
                .Which(controller => controller
                    .WithData(new Course() { Id = 1 }))
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(ERROR_NOTIFICATION))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Index", "Courses");

        [Theory]
        [InlineData(1, "Content")]
        public void CreateShouldRedirectIfCurrentUserIsNotInCourse(int courseId, string content)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Create?courseId={courseId}&content={content}")
                    .WithUser(TestUser.Identifier, TestUser.Username)
                    .WithAntiForgeryToken())
                .To<CourseReviewsController>(c => c.Create(courseId, content))
                .Which(controller => controller
                    .WithData(new Course() { Id = 1}))
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(ERROR_NOTIFICATION))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Details", "Courses", new { Id = courseId });

        [Theory]
        [InlineData(1, "Content")]
        public void CreateShouldAddNewReviewToDb(int courseId, string content)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Create?courseId={courseId}&content={content}")
                    .WithUser(TestUser.Identifier, TestUser.Username)
                    .WithAntiForgeryToken())
                .To<CourseReviewsController>(c => c.Create(courseId, content))
                .Which(controller => controller
                    .WithData(GetUserWithCourse()))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Review>(review => review
                        .Any(r =>
                                r.CourseId == courseId &&
                                r.Content == content)));

        [Theory]
        [InlineData(1)]
        public void UpdateShouldReturnView(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Update/{id}")
                    .WithUser(TestUser.Identifier, TestUser.Username)
                    .WithAntiForgeryToken())
            .To<CourseReviewsController>(c => c.Update(id))
            .Which(controller => controller
                .WithData(ReviewWithUser()))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ReviewContentServiceModel>(model => model.Content == "Content"));

        [Theory]
        [InlineData(2)]
        public void UpdateShouldRedirectIfReviewNotExist(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Update/{id}")
                    .WithUser(TestUser.Identifier, TestUser.Username)
                    .WithAntiForgeryToken())
            .To<CourseReviewsController>(c => c.Update(id))
            .Which(controller => controller
                .WithData(ReviewWithUser()))
            .ShouldHave()
            .TempData(tempData => tempData
                .ContainingEntryWithKey(ERROR_NOTIFICATION))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction("Index", "Home");

        [Theory]
        [InlineData(1, 1, "Content1")]
        public void PostUpdateShouldRedirect(int id, int courseId, string content)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Update/{id}?courseId={courseId}")
                    .WithFormField("Content", content)
                    .WithUser(TestUser.Identifier, TestUser.Username)
                    .WithAntiForgeryToken())
                .To<CourseReviewsController>(c => c.Update(id, courseId, new ReviewContentServiceModel
                {
                    Content = content,
                }))
                .Which(controller => controller
                    .WithData(ReviewWithUser()))
                .ShouldReturn()
                .RedirectToAction("Details", "Courses", new { Id = courseId });

        [Theory]
        [InlineData(1, 1, null)]
        public void PostUpdateShouldReturnViewIfModelStateIsInvalid(int id, int courseId, string content)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Update/{id}?courseId={courseId}")
                    .WithFormField("Content", content)
                    .WithUser(TestUser.Identifier, TestUser.Username)
                    .WithAntiForgeryToken())
                .To<CourseReviewsController>(c => c.Update(id, courseId, new ReviewContentServiceModel
                {
                    Content = content,
                }))
                .Which()
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData(1, 1, "Content1")]
        public void PostShouldRedirectIfCurrentUserIsNotAuthorOrAdmin(int id, int courseId, string content)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Update/{id}?courseId={courseId}")
                    .WithFormField("Content", content)
                    .WithUser("SomeId", TestUser.Username)
                    .WithAntiForgeryToken())
                    .To<CourseReviewsController>(c => c.Update(id, courseId, new ReviewContentServiceModel
                    {
                        Content = content,
                    }))
                    .Which(controller => controller
                        .WithData(ReviewWithUser()))
                    .ShouldHave()
                    .TempData(tempData => tempData
                        .ContainingEntryWithKey(ERROR_NOTIFICATION))
                    .AndAlso()
                    .ShouldReturn()
                    .RedirectToAction("Index", "Home");

        [Theory]
        [InlineData(1, 1, "Content1")]
        public void PostUpdateShouldUpdateReview(int id, int courseId, string content)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Update/{id}?courseId={courseId}")
                    .WithFormField("Content", content)
                    .WithUser(TestUser.Identifier, TestUser.Username)
                    .WithAntiForgeryToken())
                    .To<CourseReviewsController>(c => c.Update(id, courseId, new ReviewContentServiceModel
                    {
                        Content = content,
                    }))
                    .Which(controller => controller
                        .WithData(ReviewWithUser()))
                    .ShouldHave()
                    .Data(data => data
                        .WithSet<Review>(reviews => reviews
                            .Any(r => r.Content == content)));

        [Theory]
        [InlineData(1, 1)]
        public void DeleteShouldRedirect(int reviewId, int courseId)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Delete?reviewId={reviewId}&courseId={courseId}")
                    .WithUser(TestUser.Identifier, TestUser.Username)
                    .WithAntiForgeryToken())
                .To<CourseReviewsController>(c => c.Delete(reviewId, courseId))
                .Which(controller => controller
                    .WithData(ReviewWithUser()))
                .ShouldReturn()
                .RedirectToAction("Details", "Courses", new { Id = courseId });
    }
}
