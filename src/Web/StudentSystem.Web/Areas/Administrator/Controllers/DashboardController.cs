namespace StudentSystem.Web.Areas.Administrator.Controllers
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Administrator;
    using StudentSystem.Services.Course;
    using StudentSystem.Services.ExcelExport;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.User;
    using StudentSystem.Web.Areas.Administrator.Controllers.Abstaction;
    using StudentSystem.Web.Infrastructure.Extensions;

    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Web.Common.NotificationsConstants;

    [Authorize(Roles = ADMIN_ROLE)]
    public class DashboardController : AdministratorController
    {
        private readonly IAdministratorService administratorService;
        private readonly IExcelExportService excelExportService;
        private readonly ICourseService courseService;
        private readonly UserManager<ApplicationUser> userManager;

        public DashboardController(
            IAdministratorService administratorService,
            IExcelExportService excelExportService,
            ICourseService courseService,
            UserManager<ApplicationUser> userManager)
        {
            this.administratorService = administratorService;
            this.excelExportService = excelExportService;
            this.courseService = courseService;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(string search = null, int currentPage = 1)
        {
            var users = this.administratorService
                .GetUsers(search, currentPage);

            return this.View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Promotion(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            var errorMessage = await this.ValidateController(user);
            if (errorMessage != string.Empty)
            {
                this.TempData[WARNING_NOTIFICATION] = errorMessage;

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

            var errorMessage = await this.ValidateController(user);
            if (errorMessage != string.Empty)
            {
                this.TempData[ERROR_NOTIFICATION] = errorMessage;

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

        [HttpPost]
        public async Task<IActionResult> BanUser(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.TempData[ERROR_NOTIFICATION] = USER_NOT_EXIST_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            var isUserAdmin = await this.userManager.IsInRoleAsync(user, ADMIN_ROLE);
            if (isUserAdmin)
            {
                this.TempData[ERROR_NOTIFICATION] = CANNOT_BAN_ADMIN_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            var isSuccessfully = await this.administratorService.BanAsync(id);
            if (!isSuccessfully)
            {
                this.TempData[ERROR_NOTIFICATION] = USER_IS_ALREADY_BANED_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[SUCCESS_NOTIFICATION] = SUCCESSFULLY_BAN_A_USER;

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Unban(string id)
        {
            var isUserExist = await this.administratorService.IsUserExistAsync(id);
            if (!isUserExist)
            {
                this.TempData[ERROR_NOTIFICATION] = USER_NOT_EXIST_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            var isSuccessfully = await this.administratorService.UnbanAsync(id);
            if (!isSuccessfully)
            {
                this.TempData[ERROR_NOTIFICATION] = USER_IS_NOT_BANNED_MESSAGE;

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[SUCCESS_NOTIFICATION] = SUCCESSSFULLY_UNBAN_A_USER;

            return this.RedirectToAction(nameof(this.Index));
        }

        // If everything is okay, the method return a empty string. Otherwise return a error message.
        private async Task<string> ValidateController(
            ApplicationUser user,
            [CallerMemberName] string callerName = "")
        {
            if (user == null)
            {
                return USER_NOT_EXIST_MESSAGE;
            }

            if (await this.administratorService.IsUserBannedAsync(user.Id))
            {
                return callerName switch
                {
                    nameof(this.Promotion) => CANNOT_PROMOTE_A_BANNED_USER,
                    nameof(this.Demotion) => CANNOT_DEMOTE_A_BANNED_USER,
                    _ => string.Empty,
                };
            }

            var isTheSameUser = user.Id == this.User.GetId();
            if (isTheSameUser)
            {
                return CANNOT_CHANGE_OWN_ROLES_MESSAGE;
            }

            return string.Empty;
        }
    }
}