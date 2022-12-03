namespace StudentSystem.Tests.PipeLine
{
    using MyTested.AspNetCore.Mvc;

    using StudentSystem.Services.ExcelExport.Enums;
    using StudentSystem.Services.ExcelExport.Models;
    using StudentSystem.Web.Areas.Administrator.Controllers;

    using Xunit;

    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Web.Common.NotificationsConstants;
    using static StudentSystem.Tests.Data.ExportDashboardTestData;

    public class ExportDashboardTest
    {
        private const string ROUTE = "Administrator/ExportDashboard";

        public static readonly object[][] ValidParameters =
        {
            new object[] { ExportType.Course.ToString(), 1 },
            new object[] { ExportType.City.ToString(), 1 }
        };

        public static readonly object[][] InvalidParametes =
        {
            new object[] { "InvliadType", 1 },
            new object[] {ExportType.Course.ToString(), -1},
            new object[] {ExportType.City.ToString(), -1},
        };

        [Fact]
        public void IndexShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation($"{ROUTE}")
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<ExportDashboardController>(c => c.Index())
                .Which()
                .ShouldReturn()
                .View();

        [Theory, MemberData(nameof(ValidParameters))]
        public void ExportShouldReturnFile(string exportType, int entityToExport)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Export")
                    .WithFormFields(new
                    {
                        ExportType = exportType,
                        EntityToExportId = entityToExport.ToString(),
                    })
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<ExportDashboardController>(c => c.Export(new ExportDataServiceModel
                {
                    ExportType = exportType,
                    EntityToExportId = entityToExport,
                }))
                .Which(controller => controller
                    .WithData(CoursesAndCities()))
                .ShouldReturn()
                .File();

        [Theory, MemberData(nameof(InvalidParametes))]
        public void ExportShouldRedirectIfExportDataIsInvalid(string exportType, int entityToExport)
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"{ROUTE}/Export")
                    .WithFormFields(new
                    {
                        ExportType = exportType,
                        EntityToExportId = entityToExport.ToString(),
                    })
                    .WithUser(new[] { ADMIN_ROLE })
                    .WithAntiForgeryToken())
                .To<ExportDashboardController>(c => c.Export(new ExportDataServiceModel
                {
                    ExportType = exportType,
                    EntityToExportId = entityToExport,
                }))
                .Which(controller => controller
                    .WithData(CoursesAndCities())
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(ERROR_NOTIFICATION))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Index", "Home", new { area = string.Empty }));
    }
}
