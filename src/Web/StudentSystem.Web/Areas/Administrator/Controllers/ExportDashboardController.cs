namespace StudentSystem.Web.Areas.Administrator.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.ExcelExport;
    using StudentSystem.Services.ExcelExport.Models;
    using StudentSystem.Web.Areas.Administrator.Controllers.Abstaction;

    using static StudentSystem.Web.Common.GlobalConstants;

    [Authorize(Roles = ADMIN_ROLE)]
    public class ExportDashboardController : AdministratorController
    {
        private readonly IExcelExportService excelExportService;

        public ExportDashboardController(IExcelExportService excelExportService)
        {
            this.excelExportService = excelExportService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Export(ExportDataServiceModel exportData)
        {
            var (data, contentType, fileName) =
                await this.excelExportService.ExportAsync(exportData);

            return File(data, contentType, fileName);
        }
    }
}
