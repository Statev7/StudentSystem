namespace StudentSystem.Web.Controllers
{
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

    [AutoValidateAntiforgeryToken]
    public class LessonsController : Controller
	{
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
		public IActionResult Index()
		{
			var lessons = this.lessonService.GetAll<AllLessonsViewModel>();

			return View(lessons);
		}

		[HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public IActionResult Create()
		{
			var lesson = new CreateLessonBindingModel()
			{
				Begining = null,
				End = null,
				Courses = this.courseService.GetAll<CourseIdNameViewModel>()
			};

			return this.View(lesson);
		}

		[HttpPost]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Create(CreateLessonBindingModel lesson)
		{
            if (!this.ModelState.IsValid)
            {
				lesson.Courses = this.courseService.GetAll<CourseIdNameViewModel>();

                return this.View(lesson);
            }

			var isCourseExist = this.courseService.GetById<CourseIdNameViewModel>(lesson.CourseId) != null;
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

                lesson.Courses = this.courseService.GetAll<CourseIdNameViewModel>();

                return this.View(lesson);
            }

            await this.lessonService.CreateAsync(lesson);

			this.TempData[SUCCESS_NOTIFICATION] = SUCCESSFULLY_CREATED_LESSON_MESSAGE;

            return this.RedirectToAction(nameof(this.Index));
		}

		[HttpGet]
		public IActionResult Details(int id)
		{
			var lesson = this.lessonService.GetById<DetailsLessonViewModel>(id);

			return this.View(lesson);
		}
	}
}
