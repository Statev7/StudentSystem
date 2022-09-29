namespace StudentSystem.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Course;
    using StudentSystem.Services.Module;
    using StudentSystem.Web.Common;

    [AutoValidateAntiforgeryToken]
    public class ModuleController : Controller
    {
        private readonly IModuleService moduleService;
        private readonly ICourseService courseService;

        public ModuleController(
            IModuleService moduleService,
            ICourseService courseService)
        {
            this.moduleService = moduleService;
            this.courseService = courseService;
        }

        [Authorize]
        public async Task<IActionResult> Apply(int id)
        {
            var course = await this.courseService.GetByIdAsync<Course>(id);
            if (course == null)
            {
                this.TempData[NotificationsConstants.ERROR_NOTIFICATION] 
                    = NotificationsConstants.INVALID_COURSE_MESSAGE;

                return this.RedirectToAction("Index", "Home");
            }

            var user = this.User;

            var isValid = await this.moduleService.RegisterForCourseAsync(course, user);
            if (!isValid)
            {
                this.TempData[NotificationsConstants.ERROR_NOTIFICATION]
                    = NotificationsConstants.ALREADY_IN_COURSE_MESSAGE;

                return this.RedirectToAction("Index", "Home");
            }

            this.TempData[NotificationsConstants.SUCCESS_NOTIFICATION]
                = string.Format(NotificationsConstants.SUCCESSFULLY_REGISTERED_FOR_COURSE_MESSAGE, course.Name);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
