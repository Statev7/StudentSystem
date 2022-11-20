namespace StudentSystem.Services.ExcelExport
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    using StudentSystem.Services.ExcelExport.Models;
    using StudentSystem.Web.Data;

    public class ExcelExportService : IExcelExportService
    {
        private readonly StudentSystemDbContext dbContext;
        private readonly IMapper mapper;

        public ExcelExportService(StudentSystemDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<(byte[] data, string contentType, string fileName)> ExportStudentsByCourseAsync(int courseId)
        {
            var students = await this.dbContext
                    .Users
                    .Where(u => u.UserCourses
                        .Any(c => c.CourseId == courseId))
                    .OrderBy(u => u.FirstName)
                    .ProjectTo<StudentsFromCourseExportServiceModel>(this.mapper.ConfigurationProvider)
                    .ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("Report");
            var range = worksheet.Cells["A1"].LoadFromCollection(students, true);
            range.AutoFitColumns();

            worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(1).Style.Font.Bold = true;

            var excelData = package.GetAsByteArray();
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "Report.xlsx";

            return (excelData, contentType, fileName);
        }
    }
}
