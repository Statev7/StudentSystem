namespace StudentSystem.Web.Areas.Administrator.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.ExcelExport;
    using StudentSystem.Services.ExcelExport.Models;
    using StudentSystem.Web.Areas.Administrator.Controllers.Abstaction;

    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Web.Common.NotificationsConstants;

    [Authorize(Roles = ADMIN_ROLE)]
    public class ExportDashboardController : AdministratorController
    {
        private readonly IExcelExportService excelExportService;

        public ExportDashboardController(IExcelExportService excelExportService) 
            => this.excelExportService = excelExportService;

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Export(ExportDataServiceModel exportData)
        {
            try
            {
                var (data, contentType, fileName) =
                        await this.excelExportService.ExportAsync(exportData);

                return this.File(data, contentType, fileName);
            }
            catch (ArgumentException ae)
            {
                this.TempData[ERROR_NOTIFICATION] = ae.Message;

                return this.RedirectToAction("Index", "Home", new {area = string.Empty});
            }
        }
    }
}
