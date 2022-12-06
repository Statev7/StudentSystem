namespace StudentSystem.Tests.PipeLine
{
    using MyTested.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.Web.Controllers.ApiControllers;

    using Xunit;

    public class LessonsApiControllerTests
    {
        [Theory]
        [InlineData(1)]
        public void DetailsShouldReturnModel(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"api/lessons?id={id}")
                    .WithUser())
                .To<LessonsApiController>(c => c.GetDetails(id))
                .Which(controller => controller
                    .WithData(new Lesson()))
                .ShouldReturn()
                .ResultOfType<LessonDetailsViewModel>();
    }
}
