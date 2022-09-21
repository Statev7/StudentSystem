namespace StudentSystem.Web.Controllers
{
	using System.Threading.Tasks;

	using Microsoft.AspNetCore.Mvc;

	using StudentSystem.Services.Course;
	using StudentSystem.Services.Lesson;
	using StudentSystem.ViewModels.Course;
	using StudentSystem.ViewModels.Lesson;
	using StudentSystem.Web.Common;
	using StudentSystem.Web.Infrastructure.Helpers;

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
				this.TempData[NotificationsConstants.ERROR_NOTIFICATION] 
					= NotificationsConstants.INVALID_COURSE_MESSAGE;

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
				this.TempData[NotificationsConstants.ERROR_NOTIFICATION] = result.errorMessage;

                lesson.Courses = this.courseService.GetAll<CourseIdNameViewModel>();

                return this.View(lesson);
            }

            await this.lessonService.CreateAsync(lesson);

			this.TempData[NotificationsConstants.SUCCESS_NOTIFICATION] 
				= NotificationsConstants.SUCCESSFULLY_CREATED_LESSON_MESSAGE;

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
