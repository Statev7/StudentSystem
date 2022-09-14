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

        [HttpGet]
        public IActionResult Update(int id)
        {
            var courseToUpdate = this.courseService.GetById<UpdateCourseBindingModel>(id);
            if (!this.ModelState.IsValid)
            {
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
                this.RedirectToAction("Index", "Home");
            }

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var course = this.courseService.GetById<DetailCourseViewModel>(id);
            if (course == null)
            {
                return this.RedirectToAction("Index");
            }

            return this.View(course);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.courseService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
