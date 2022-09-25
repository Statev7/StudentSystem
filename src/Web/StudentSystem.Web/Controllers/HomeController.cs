namespace StudentSystem.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Home;
    using StudentSystem.Web.Infrastructure.Extensions;
    using StudentSystem.Web.Models;

    public class HomeController : Controller
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        public IActionResult Index()
        {
            var userId = this.User.GetId();
            var information = this.homeService.GetInformation(userId);

            return View(information);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
