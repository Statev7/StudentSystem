namespace StudentSystem.Web.Controllers
{
	using System.Threading.Tasks;

	using Microsoft.AspNetCore.Mvc;

	using StudentSystem.Services.Lesson;
	using StudentSystem.ViewModels.Lesson;

	public class LessonsController : Controller
	{
		private readonly ILessonService lessonService;

		public LessonsController(ILessonService lessonService)
		{
			this.lessonService = lessonService;
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
			var lesson = this.lessonService.GetViewModelForCreate();

			return this.View(lesson);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateLessonBindingModel lesson)
		{
            if (!this.ModelState.IsValid)
            {
				lesson.Courses = this.lessonService.GetAllCourses();
                return this.View(lesson);
            }

            await this.lessonService.CreateAsync(lesson);

			return this.RedirectToAction(nameof(this.Index));
		}
	}
}
