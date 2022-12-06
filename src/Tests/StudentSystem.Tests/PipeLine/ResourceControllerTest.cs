namespace StudentSystem.Tests.PipeLine
{
    using MyTested.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Resource.Models;
    using StudentSystem.ViewModels.Resource;
    using StudentSystem.Web.Areas.Trainings.Controllers;

    using System.Linq;

    using Xunit;

    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Tests.Data.ResourceTestData;

    public class ResourceControllerTest
    {
        private const string ROUTE = "Trainings/Resources";

        public static readonly object[][] ValidParametes =
        {
            new object[]
            {
                "ResourceName",
                "https://docs.google.com/document/d/1VE5kby49hrV5XU4MWevWGpt8iLwEvF40/edit?usp=sharing&ouid=106750394811837266265&rtpof=true&sd=true",
                1
            }
        };

        public static readonly object[][] ValidParametesForUpdate =
        {
            new object[]
            {
                1,
                "ResourceName",
                "https://docs.google.com/document/d/1VE5kby49hrV5XU4MWevWGpt8iLwEvF40/edit?usp=sharing&ouid=106750394811837266265&rtpof=true&sd=true",
                2,
            }
        };

        [Theory]
        [InlineData(1)]
        public void IndexShouldReturnViewWithCorrectModel(int currentPage)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}?currentPage={currentPage}")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<ResourcesController>(c => c.Index(currentPage))
                .Which(controller => controller
                    .WithData(new Resource()))
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<PageResourceViewModel>(model =>
                        model.Resources.Count() == 1));

        [Fact]
        public void CreateShouldReturnViewWithCorrectModel()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Create")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<ResourcesController>(c => c.Create())
                .Which(controller => controller
                    .WithData(Lessons()))
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<ResourceFormServiceModel>(model =>
                        model.Lessons.Count() == 10));

        [Theory, MemberData(nameof(ValidParametes))]
        public void PostCreateShouldAddNewResourceToDb(string name, string url, int lessonId)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Create")
                    .WithFormFields(new
                    {
                        Name = name,
                        URL = url,
                        LessonId = lessonId.ToString()
                    })
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<ResourcesController>(c => c.Create(new ResourceFormServiceModel
                {
                    Name = name,
                    URL = url,
                    LessonId = lessonId
                }))
                .Which(controller => controller
                    .WithData(Lessons()))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Resource>(resources => resources
                        .Any(r =>
                            r.Name == name &&
                            r.URL == url &&
                            r.LessonId == lessonId)));

        [Theory]
        [InlineData(1)]
        public void UpdateShouldReturnViewWithCorrectModel(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Update?id={id}")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<ResourcesController>(c => c.Update(id))
                .Which(controller => controller
                    .WithData(ResourcesForUpdate()))
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<ResourceFormServiceModel>(model =>
                    {
                        Assert.Equal("SomeResource", model.Name);
                        Assert.Equal(10, model.Lessons.Count());
                    }));

        [Theory, MemberData(nameof(ValidParametesForUpdate))]
        public void PostUpdateShouldUpdateModel(int id, string name, string url, int lessonId)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Update?id={id}")
                    .WithFormFields(new
                    {
                        Name = name,
                        URL = url,
                        LessonId = lessonId.ToString()
                    })
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<ResourcesController>(c => c.Update(id, new ResourceFormServiceModel
                {
                    Name = name,
                    URL = url,
                    LessonId = lessonId
                }))
                .Which(controller => controller
                    .WithData(ResourcesForUpdate()))
            .ShouldHave()
            .Data(data => data
                .WithSet<Resource>(resources => resources
                    .Any(r =>
                        r.Name == name &&
                        r.URL == url &&
                        r.LessonId == lessonId)));

        [Theory]
        [InlineData(1)]
        public void DetailsShouldReturnViewWithCorrectModelIfUserWithAdminRoleTryToOpenIt(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Details?id={id}")
                    .WithUser(TestUser.Identifier, TestUser.Username, ADMIN_ROLE))
                .To<ResourcesController>(r => r.Details(id))
                .Which(controller => controller
                    .WithData(ResourceForDetails())
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<ResouceDetailsViewModel>(model =>
                    {
                        Assert.Equal("SomeResource", model.Name);
                    })));

        [Theory]
        [InlineData(1)]
        public void DetailsShouldReturnViewWithCorrectModelIfUserIsRegistratedForCourseWhoHasThisResource(int id)
        => MyMvc
            .Pipeline()
            .ShouldMap(request => request
                .WithLocation($"{ROUTE}/Details?id={id}")
                .WithUser(TestUser.Identifier, TestUser.Username, ADMIN_ROLE))
            .To<ResourcesController>(c => c.Details(id))
            .Which(controller => controller
                .WithData(ResourceForDetails())
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ResouceDetailsViewModel>(model =>
                {
                    Assert.Equal("SomeResource", model.Name);
                })));

        [Theory]
        [InlineData(1)]
        public void DeleteShouldMarkResourceAsDeleted(int id)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}/Delete?id={id}")
                    .WithUser(new[] { ADMIN_ROLE }))
                .To<ResourcesController>(c => c.Delete(id))
                .Which(controller => controller
                    .WithData(new Resource { Id = 1 }))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Resource>(resource => resource
                        .Any(r =>
                            r.Id == id &&
                            r.IsDeleted)));
    }
}
