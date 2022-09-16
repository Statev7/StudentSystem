namespace StudentSystem.Web.Controllers
{
	using System.Threading.Tasks;

	using Microsoft.AspNetCore.Mvc;

	using StudentSystem.Services.Course;
	using StudentSystem.Services.Lesson;
	using StudentSystem.ViewModels.Course;
	using StudentSystem.ViewModels.Lesson;

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

			var isCourseValid = this.courseService.GetById<CourseIdNameViewModel>(lesson.CourseId) != null;
            if (!isCourseValid)
			{
				this.RedirectToAction("Index", "Home");
			}

            await this.lessonService.CreateAsync(lesson);

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
