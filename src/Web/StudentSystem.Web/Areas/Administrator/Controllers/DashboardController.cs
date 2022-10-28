namespace StudentSystem.Web.Areas.Administrator.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Administrator;
    using StudentSystem.Web.Areas.Administrator.Controllers.Abstaction;
    using StudentSystem.Web.Infrastructure.Extensions;

    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Web.Common.NotificationsConstants;

    [Authorize(Roles = ADMIN_ROLE)]
    public class DashboardController : AdministratorController
    {
        private readonly IAdministratorService administratorService;
        private readonly UserManager<ApplicationUser> userManager;

        public DashboardController(
            IAdministratorService administratorService,
            UserManager<ApplicationUser> userManager)
        {
            this.administratorService = administratorService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await this.administratorService
                .GetAllUsersAsync();

            return this.View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Promotion(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                this.TempData[ERROR_NOTIFICATION] = USER_NOT_EXIST_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            var currentUserId = this.User.GetId();
            if (currentUserId == id)
            {
                this.TempData[ERROR_NOTIFICATION] = CANNOT_CHANGE_OWN_ROLES_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            var isSuccesfully = await this.administratorService.PromoteAsync(user);
            if (!isSuccesfully)
            {
                this.TempData[WARNING_NOTIFICATION] = UNSUCCESSFULLY_PROMOTED_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[SUCCESS_NOTIFICATION] = SUCCESSFULLY_PROMOTED_MESSAGE;

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Demotion(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                this.TempData[ERROR_NOTIFICATION] = USER_NOT_EXIST_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            var currentUserId = this.User.GetId();
            if (currentUserId == id)
            {
                this.TempData[ERROR_NOTIFICATION] = CANNOT_CHANGE_OWN_ROLES_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            var isSuccesfully = await this.administratorService.DemoteAsync(user);
            if (!isSuccesfully)
            {
                this.TempData[WARNING_NOTIFICATION] = UNSUCCESSFULYY_DEMOTE_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[SUCCESS_NOTIFICATION] = SUCCESSFULLY_DEMOTE_MESSSAGE;

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
