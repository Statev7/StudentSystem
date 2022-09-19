namespace StudentSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Course;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.Web.Common;
    using StudentSystem.Web.Infrastructure.Helpers;

    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;

        public CoursesController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var courses = this.courseService.GetAll<AllCoursesViewModel>();

            return View(courses);
        }

        [HttpGet]
        public IActionResult Create()
            => this.View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseBindingModel course)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(course);
            }

            //If the second date is earlier that the first, the method return false
            var isValid = MyValidator.CompareDates(course.StartDate, course.EndDate);
            if (!isValid)
            {
                this.TempData[NotificationsConstants.ERROR_NOTIFICATION] = 
                    string.Format(NotificationsConstants.INVALID_DATE_MESSAGE, "End date", "start date");

                return this.View(course);
            }

            await this.courseService.CreateAsync(course);

            this.TempData[NotificationsConstants.SUCCESS_NOTIFICATION] =
                string.Format(NotificationsConstants.SUCCESSFULLY_CREATED_COURSE_MESSAGE, course.Name);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var courseToUpdate = this.courseService.GetById<UpdateCourseBindingModel>(id);
            if (courseToUpdate == null)
            {
                this.TempData[NotificationsConstants.ERROR_NOTIFICATION]
                    = NotificationsConstants.INVALID_COURSE_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(courseToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCourseBindingModel courseToUpdate)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(courseToUpdate);
            }

            var isUpdated = await courseService.UpdateAsync(courseToUpdate);
            if (!isUpdated)
            {
                this.TempData[NotificationsConstants.ERROR_NOTIFICATION] 
                    = NotificationsConstants.INVALID_COURSE_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[NotificationsConstants.SUCCESS_NOTIFICATION]
                = string.Format(NotificationsConstants.SUCCESSFULLY_UPDATE_COURSE_MESSAGE, courseToUpdate.Name);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var course = this.courseService.GetDetails(id);
            if (course == null)
            {
                this.TempData[NotificationsConstants.ERROR_NOTIFICATION] 
                    = NotificationsConstants.INVALID_COURSE_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(course);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.courseService.DeleteAsync(id);
            if (!isDeleted)
            {
                this.TempData[NotificationsConstants.ERROR_NOTIFICATION] 
                    = NotificationsConstants.INVALID_COURSE_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[NotificationsConstants.SUCCESS_NOTIFICATION]
                = NotificationsConstants.SUCCESSFULLY_DELETE_COURSE_MESSAGE;

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
