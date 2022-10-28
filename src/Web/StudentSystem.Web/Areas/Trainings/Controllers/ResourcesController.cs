namespace StudentSystem.Web.Areas.Trainings.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Services.Lesson;
    using StudentSystem.Services.Resource;
    using StudentSystem.Services.Resource.Models;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.Web.Areas.Trainings.Controllers.Abstraction;

    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Web.Common.NotificationsConstants;

    [Authorize(Roles = ADMIN_ROLE)]
    public class ResourcesController : TrainingController
    {
        private readonly IResourceService resourceService;
        private readonly ILessonService lessonService;

        public ResourcesController(IResourceService resourceService, ILessonService lessonService)
        {
            this.resourceService = resourceService;
            this.lessonService = lessonService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var resourceModel = new ResourceFormServiceModel()
            {
                Lessons = await this.lessonService
                .GetAllAsQueryable<LessonIdNameViewModel>()
                .OrderBy(l => l.Title)
                .ToListAsync(),
            };

            return View(resourceModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ResourceFormServiceModel resource)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(resource);
            }

            var isLessonExist = await this.lessonService.IsExistAsync(resource.LessonId);
            if (!isLessonExist)
            {
                this.TempData[ERROR_NOTIFICATION] = 
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, LESSON_KEYWORD);

                return this.RedirectToAction("Index", "Home", new { area = string.Empty });
            }

            await this.resourceService.CreateEntityAsync(resource);

            this.TempData[SUCCESS_NOTIFICATION] =
                string.Format(SUCCESSFULLY_CREATED_ENTITY_MESSAGE, resource.Name, RESOURCE_KEYWORD);

            return this.RedirectToAction("Index", "Home", new { area = string.Empty });
        }
    }
}
