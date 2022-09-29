namespace StudentSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Course;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.Web.Infrastructure.Helpers;
    using StudentSystem.Services.Course.Models;

    using static StudentSystem.Web.Common.NotificationsConstants;
    using static StudentSystem.Web.Common.GlobalConstants;
    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Web.Common;

    [AutoValidateAntiforgeryToken]
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;

        public CoursesController(
            ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var courses = this.courseService
                .GetAllAsQueryable<ListCoursesViewModel>()
                .Where(c => c.StartDate > DateTime.UtcNow)
                .ToList();

            return View(courses);
        }

        [HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public IActionResult Create()
            => this.View();

        [HttpPost]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Create(CourseFormServiceModel course)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(course);
            }

            var result = MyValidator
                .ValidateDates(course.StartDate, course.EndDate, "Start date", "End date");

            if (!result.isValid)
            {
                this.TempData[ERROR_NOTIFICATION] = result.errorMessage;

                return this.View(course);
            }

            await this.courseService.CreateAsync(course);

            this.TempData[SUCCESS_NOTIFICATION] = 
                string.Format(SUCCESSFULLY_CREATED_COURSE_MESSAGE, course.Name);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Update(int id)
        {
            var courseToUpdate = await this.courseService.GetByIdAsync<CourseFormServiceModel>(id);
            if (courseToUpdate == null)
            {
                this.TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(courseToUpdate);
        }

        [HttpPost]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Update(int id, CourseFormServiceModel courseToUpdate)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(courseToUpdate);
            }

            var isUpdated = await courseService.UpdateAsync(id, courseToUpdate);
            if (!isUpdated)
            {
                this.TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[SUCCESS_NOTIFICATION]
                = string.Format(SUCCESSFULLY_UPDATE_COURSE_MESSAGE, courseToUpdate.Name);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var course = this.courseService.GetDetails(id);
            if (course == null)
            {
                this.TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(course);
        }

        [HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.courseService.DeleteAsync(id);
            if (!isDeleted)
            {
                this.TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[SUCCESS_NOTIFICATION] = SUCCESSFULLY_DELETE_COURSE_MESSAGE;

            return this.RedirectToAction(nameof(this.Index));
        }

        [Authorize]
        public async Task<IActionResult> Apply(int id)
        {
            var course = await this.courseService.GetByIdAsync<CourseIdNameViewModel>(id);
            if (course == null)
            {
                this.TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return this.RedirectToAction("Index", "Home");
            }

            var user = this.User;

            var isValid = await this.courseService.RegisterForCourseAsync(id, user);
            if (!isValid)
            {
                this.TempData[ERROR_NOTIFICATION] = ALREADY_IN_COURSE_MESSAGE;

                return this.RedirectToAction("Index", "Home");
            }

            this.TempData[SUCCESS_NOTIFICATION]
                = string.Format(SUCCESSFULLY_REGISTERED_FOR_COURSE_MESSAGE, course.Name);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
