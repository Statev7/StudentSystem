namespace StudentSystem.Web.Areas.Trainings.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Category;
    using StudentSystem.Services.Course;
    using StudentSystem.Services.Course.Models;
    using StudentSystem.ViewModels.Category;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.Web.Areas.Trainings.Controllers.Abstraction;
    using StudentSystem.Web.Infrastructure.Helpers;

    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Web.Common.NotificationsConstants;

    [AutoValidateAntiforgeryToken]
    public class CoursesController : TrainingController
    {
        private const int CORSES_PER_PAGE = 6;

        private readonly ICourseService courseService;
        private readonly ICategoryService categoryService;

        public CoursesController(
            ICourseService courseService,
            ICategoryService categoryService) 
        {
            this.courseService = courseService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index(int[] filters, int currentPage = 1)
        {
            var courses = this.courseService.GetAllCoursesPaged(filters, currentPage, CORSES_PER_PAGE);

            return View(courses);
        }

        [HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public IActionResult Create()
        {
            var courseModel = new CourseFormServiceModel()
            {
                StartDate = null,
                EndDate = null,
                Categories = this.categoryService
                        .GetAllAsQueryable<CategoryIdNameViewModel>()
                        .ToList(),
            };

            return this.View(courseModel);
        }

        [HttpPost]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Create(CourseFormServiceModel course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            var result = MyValidator
                .ValidateDates(course.StartDate.Value, course.EndDate.Value, "Start date", "End date");

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

            courseToUpdate.Categories = this.categoryService
                .GetAllAsQueryable<CategoryIdNameViewModel>()
                .ToList();

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
