namespace StudentSystem.Web.Areas.Trainings.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Course;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.Web.Areas.Trainings.Controllers.Abstraction;

    using static StudentSystem.Web.Common.NotificationsConstants;

    [Authorize]
    public class RegisterForCourseController : TrainingController
    {
        private readonly ICourseService courseService;

        public RegisterForCourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> Apply(int id)
        {
            var course = await this.courseService
                .GetByIdAsync<CourseNameViewModel>(id);

            if (course == null)
            {
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, COURSE_KEYWORD);

                return this.RedirectToAction("Index", "Courses");
            }

            var user = User;

            var isValid = await this.courseService.RegisterForCourseAsync(id, user);
            if (!isValid)
            {
                this.TempData[ERROR_NOTIFICATION] = ALREADY_IN_COURSE_MESSAGE;

                return this.RedirectToAction("Index", "Courses");
            }

            this.TempData[SUCCESS_NOTIFICATION]
                = string.Format(SUCCESSFULLY_REGISTERED_FOR_COURSE_MESSAGE, course.Name);

            return this.RedirectToAction("Details", "Courses", new { id });
        }
    }
}
