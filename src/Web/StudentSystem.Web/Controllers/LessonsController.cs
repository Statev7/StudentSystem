namespace StudentSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using StudentSystem.Services.Course;
	using StudentSystem.Services.Lesson;
	using StudentSystem.ViewModels.Course;
	using StudentSystem.ViewModels.Lesson;
	using StudentSystem.Web.Infrastructure.Helpers;

    using static StudentSystem.Web.Common.NotificationsConstants;
    using static StudentSystem.Web.Common.GlobalConstants;
	using StudentSystem.Services.Lesson.Models;

	[AutoValidateAntiforgeryToken]
	[Authorize(Roles = ADMIN_ROLE)]
    public class LessonsController : Controller
	{
		private const int LESSON_PER_PAGE = 6;

		private readonly ILessonService lessonService;
		private readonly ICourseService courseService;

		public LessonsController(
			ILessonService lessonService, 
			ICourseService courseService)
		{
			this.lessonService = lessonService;
			this.courseService = courseService;
		}

		[HttpGet]
		public IActionResult Index(int courseId, int currentPage = 1)
		{
			var lessons = this.lessonService.GetAllLessonsPaged(courseId, currentPage, LESSON_PER_PAGE);

			return View(lessons);
		}

		[HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public IActionResult Create()
		{
			var lesson = new LessonFormServiceModel()
			{
				Begining = null,
				End = null,
				Courses = this.courseService
					.GetAllAsQueryable<CourseIdNameViewModel>()
					.ToList()
			};

			return this.View(lesson);
		}

		[HttpPost]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Create(LessonFormServiceModel lesson)
		{
            if (!this.ModelState.IsValid)
            {
				lesson.Courses = this.courseService
					.GetAllAsQueryable<CourseIdNameViewModel>()
					.ToList();

                return this.View(lesson);
            }

			var isCourseExist = await this.courseService.GetByIdAsync<CourseIdNameViewModel>(lesson.CourseId) != null;
            if (!isCourseExist)
			{
				this.TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            var result = MyValidator.ValidateDates(
                lesson.Begining.Value,
                lesson.End.Value,
                nameof(lesson.Begining),
                nameof(lesson.End), 
				true);

            if (!result.isValid)
            {
				this.TempData[ERROR_NOTIFICATION] = result.errorMessage;

                lesson.Courses = this.courseService
					.GetAllAsQueryable<CourseIdNameViewModel>()
					.ToList();

                return this.View(lesson);
            }

            await this.lessonService.CreateAsync(lesson);

			this.TempData[SUCCESS_NOTIFICATION] = SUCCESSFULLY_CREATED_LESSON_MESSAGE;

            return this.RedirectToAction(nameof(this.Index));
		}

		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			var lesson = await this.lessonService.GetByIdAsync<LessonFormServiceModel>(id);

			if (lesson == null)
			{
                this.TempData[ERROR_NOTIFICATION] = INVALID_LESSON_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

			lesson.Courses = this.courseService.GetAllAsQueryable<CourseIdNameViewModel>().ToList();

			return this.View(lesson);
		}

		[HttpPost]
		public async Task<IActionResult> Update(int id, LessonFormServiceModel lesson)
		{
			if (!ModelState.IsValid)
			{
				lesson.Courses = this.courseService
					.GetAllAsQueryable<CourseIdNameViewModel>()
					.ToList();

				return this.View(lesson);
			}

			var isUpdated = await this.lessonService.UpdateAsync(id, lesson);
			if (!isUpdated)
			{
				this.TempData[WARNING_NOTIFICATION] = INVALID_LESSON_MESSAGE;

				return this.RedirectToAction(nameof(this.Index));
            }

			this.TempData[SUCCESS_NOTIFICATION] = string.Format(SUCCESSFULLY_UPDATE_LESSON_MESSAGE, lesson.Title);

			return this.RedirectToAction(nameof(this.Index));
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var lesson = await this.lessonService.GetByIdAsync<DetailsLessonViewModel>(id);

			return this.View(lesson);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var isDeleted = await this.lessonService.DeleteAsync(id);

			if (!isDeleted)
			{
                this.TempData[WARNING_NOTIFICATION] = INVALID_LESSON_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

			this.TempData[SUCCESS_NOTIFICATION] = SUCCESSFULLY_DELETE_LESSON_MESSAGE;

            return this.RedirectToAction(nameof(this.Index));
        }
	}
}
