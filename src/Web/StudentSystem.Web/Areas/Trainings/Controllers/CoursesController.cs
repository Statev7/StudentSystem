namespace StudentSystem.Web.Areas.Trainings.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Course;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.Services.Course.Models;
    using StudentSystem.Web.Infrastructure.Helpers;
    using StudentSystem.Web.Areas.Trainings.Controllers.Abstraction;

    using static StudentSystem.Web.Common.NotificationsConstants;
    using static StudentSystem.Web.Common.GlobalConstants;


    [AutoValidateAntiforgeryToken]
    public class CoursesController : TrainingController
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
            var courses = courseService
                .GetAllAsQueryable<CourseViewModel>()
                .Where(c => c.StartDate > DateTime.UtcNow)
                .ToList();

            return View(courses);
        }

        [HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public IActionResult Create()
            => View();

        [HttpPost]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Create(CourseFormServiceModel course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            var result = MyValidator
                .ValidateDates(course.StartDate, course.EndDate, "Start date", "End date");

            if (!result.isValid)
            {
                TempData[ERROR_NOTIFICATION] = result.errorMessage;

                return View(course);
            }

            await courseService.CreateAsync(course);

            TempData[SUCCESS_NOTIFICATION] =
                string.Format(SUCCESSFULLY_CREATED_COURSE_MESSAGE, course.Name);

            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Update(int id)
        {
            var courseToUpdate = await courseService.GetByIdAsync<CourseFormServiceModel>(id);
            if (courseToUpdate == null)
            {
                TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return RedirectToAction(nameof(this.Index));
            }

            return View(courseToUpdate);
        }

        [HttpPost]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Update(int id, CourseFormServiceModel courseToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(courseToUpdate);
            }

            var isUpdated = await courseService.UpdateAsync(id, courseToUpdate);
            if (!isUpdated)
            {
                TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return RedirectToAction(nameof(this.Index));
            }

            TempData[SUCCESS_NOTIFICATION]
                = string.Format(SUCCESSFULLY_UPDATE_COURSE_MESSAGE, courseToUpdate.Name);

            return RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var course = courseService.GetDetails(id);
            if (course == null)
            {
                TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return RedirectToAction(nameof(this.Index));
            }

            return View(course);
        }

        [HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await courseService.DeleteAsync(id);
            if (!isDeleted)
            {
                TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return RedirectToAction(nameof(this.Index));
            }

            TempData[SUCCESS_NOTIFICATION] = SUCCESSFULLY_DELETE_COURSE_MESSAGE;

            return RedirectToAction(nameof(this.Index));
        }

        [Authorize]
        public async Task<IActionResult> Apply(int id)
        {
            var course = await courseService.GetByIdAsync<CourseIdNameViewModel>(id);
            if (course == null)
            {
                TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return RedirectToAction(nameof(this.Index));
            }

            var user = User;

            var isValid = await courseService.RegisterForCourseAsync(id, user);
            if (!isValid)
            {
                TempData[ERROR_NOTIFICATION] = ALREADY_IN_COURSE_MESSAGE;

                return RedirectToAction(nameof(this.Index));
            }

            TempData[SUCCESS_NOTIFICATION]
                = string.Format(SUCCESSFULLY_REGISTERED_FOR_COURSE_MESSAGE, course.Name);

            return RedirectToAction(nameof(this.Index));
        }
    }
}
