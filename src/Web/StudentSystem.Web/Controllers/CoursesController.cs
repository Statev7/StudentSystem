namespace StudentSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Course;
    using StudentSystem.ViewModels.Course;

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
            var courses = this.courseService.GetAll();

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

            await this.courseService.CreateAsync(course);

            return this.RedirectToAction("Index");
        }
    }
}
