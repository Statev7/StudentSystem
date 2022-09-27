namespace StudentSystem.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.ViewModels.Resource;
    using StudentSystem.Web.Data;

    public class ResourcesController : Controller
    {
        private readonly StudentSystemDbContext dbContext;

        public ResourcesController(StudentSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var resouces = this.dbContext.Resources.Select(r => new ListResourceViewModel
            {
                Name = r.Name,
                URL = r.URL
            })
            .ToList();

            return View(resouces);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateResourceBindingModel resource)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            var resourceToCreate = new Resource
            {
                Name = resource.Name,
                URL = resource.URL
            };

            this.dbContext.Resources.Add(resourceToCreate);
            this.dbContext.SaveChanges();

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
