namespace StudentSystem.Web.Areas.Trainings.Controllers
{
    using System.Collections.Generic;
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
        private const int RESOURCES_PER_PAGE = 6;

        private readonly IResourceService resourceService;
        private readonly ILessonService lessonService;

        public ResourcesController(IResourceService resourceService, ILessonService lessonService)
        {
            this.resourceService = resourceService;
            this.lessonService = lessonService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var resources = await this
                .resourceService
                .GetAllResourcesPagedAsync(currentPage, RESOURCES_PER_PAGE);

            return this.View(resources);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var resourceModel = new ResourceFormServiceModel()
            {
                Lessons = await this.GetAllLessons(),
            };

            return View(resourceModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ResourceFormServiceModel resource)
        {
            if (!this.ModelState.IsValid)
            {
                resource.Lessons = await this.GetAllLessons();

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

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var resource = await this.resourceService.GetByIdAsync<ResourceFormServiceModel>(id);

            if (resource == null)
            {
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, RESOURCE_KEYWORD);

                return this.RedirectToAction(nameof(this.Index));
            }

            resource.Lessons = await this.GetAllLessons();

            return this.View(resource);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ResourceFormServiceModel resource)
        {
            if (!this.ModelState.IsValid)
            {
                resource.Lessons = await this.GetAllLessons();

                return this.View(resource);
            }

            var isUpdated = await this.resourceService.UpdateEntityAsync(id, resource);
            if (!isUpdated)
            {
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, RESOURCE_KEYWORD);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[SUCCESS_NOTIFICATION] =
                string.Format(SUCCESSFULLY_UPDATED_ENTITY_MESSAGE, resource.Name, RESOURCE_KEYWORD);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var isResourceExist = await this.resourceService.IsExistAsync(id);
            if (!isResourceExist)
            {
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, RESOURCE_KEYWORD);

                return this.RedirectToAction(nameof(this.Index));
            }

            await this.resourceService.DeleteAsync(id);
            this.TempData[SUCCESS_NOTIFICATION] = SUCCESSFULLY_DELETED_ENTITY_MESSAGE;

            return this.RedirectToAction(nameof(this.Index));
        }

        private async Task<IEnumerable<LessonIdNameViewModel>> GetAllLessons()
            => await this.lessonService
            .GetAllAsQueryable<LessonIdNameViewModel>()
            .OrderBy(l => l.Title)
            .ToListAsync();
    }
}