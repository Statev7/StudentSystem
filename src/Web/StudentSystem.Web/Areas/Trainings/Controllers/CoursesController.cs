namespace StudentSystem.Web.Areas.Trainings.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Services.Category;
    using StudentSystem.Services.Course;
    using StudentSystem.Services.Course.Models;
    using StudentSystem.ViewModels.Category;
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
        public async Task<IActionResult> Index(int[] filters, int currentPage = 1)
        {
            var courses = await this.courseService
                .GetAllCoursesPagedAsync(filters, currentPage, CORSES_PER_PAGE);

            return this.View(courses);
        }

        [HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Create()
        {
            var categories = await this.categoryService
                        .GetAllAsQueryable<CategoryIdNameViewModel>()
                        .ToListAsync();

            var courseModel = new CourseFormServiceModel()
            {
                StartDate = null,
                EndDate = null,
                Categories = categories,
            };

            return this.View(courseModel);
        }

        [HttpPost]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Create(CourseFormServiceModel course)
        {
            if (!ModelState.IsValid)
            {
                return this.View(course);
            }

            var result = MyValidator
                .ValidateDates(course.StartDate.Value, course.EndDate.Value, "Start date", "End date");

            if (!result.isValid)
            {
                this.TempData[ERROR_NOTIFICATION] = result.errorMessage;

                return this.View(course);
            }

            await this.courseService.CreateAsync(course);

            this.TempData[SUCCESS_NOTIFICATION] =
                string.Format(SUCCESSFULLY_CREATED_ENTITY_MESSAGE, course.Name, COURSE_KEYWORD);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Update(int id)
        {
            var courseToUpdate = await this.courseService
                .GetByIdAsync<CourseFormServiceModel>(id);

            if (courseToUpdate == null)
            {
                this.TempData[ERROR_NOTIFICATION] = 
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, COURSE_KEYWORD);

                return this.RedirectToAction(nameof(this.Index));
            }

            courseToUpdate.Categories = await this.categoryService
                .GetAllAsQueryable<CategoryIdNameViewModel>()
                .ToListAsync();

            return this.View(courseToUpdate);
        }

        [HttpPost]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Update(int id, CourseFormServiceModel courseToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return this.View(courseToUpdate);
            }

            var isUpdated = await this.courseService.UpdateAsync(id, courseToUpdate);
            if (!isUpdated)
            {
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, COURSE_KEYWORD);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[SUCCESS_NOTIFICATION]
                = string.Format(SUCCESSFULLY_UPDATED_ENTITY_MESSAGE, courseToUpdate.Name, COURSE_KEYWORD);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var course = await this.courseService.GetDetailsAsync(id);
            if (course == null)
            {
                this.TempData[ERROR_NOTIFICATION] = 
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, COURSE_KEYWORD);

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
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, COURSE_KEYWORD);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[SUCCESS_NOTIFICATION] = SUCCESSFULLY_DELETED_ENTITY_MESSAGE;

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
