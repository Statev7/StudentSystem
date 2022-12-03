namespace StudentSystem.Services.ExcelExport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Reflection.Metadata;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;

    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    using StudentSystem.Services.City;
    using StudentSystem.Services.Course;
    using StudentSystem.Services.ExcelExport.Enums;
    using StudentSystem.Services.ExcelExport.Models;
    using StudentSystem.Web.Data;

    public class ExcelExportService : IExcelExportService
    {
        private readonly StudentSystemDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ICourseService courseService;
        private readonly ICityService cityService;

        public ExcelExportService(
            StudentSystemDbContext dbContext, 
            IMapper mapper,
            ICourseService courseService,
            ICityService cityService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.courseService = courseService;
            this.cityService = cityService;
        }

        public async Task<(byte[] data, string contentType, string fileName)> ExportAsync(ExportDataServiceModel exportData)
        {
            await this.ValidateExportData(exportData);

            var query = this.dbContext.Users.AsQueryable();

            if (exportData.ExportType == ExportType.Course.ToString())
            {
                query = query.Where(u => u.UserCourses
                                .Any(c => c.CourseId == exportData.EntityToExportId));
            }
            else if(exportData.ExportType == ExportType.City.ToString())
            {
                query = query.Where(u => u.CityId == exportData.EntityToExportId);
            }

            var collection = await query
                    .OrderBy(u => u.FirstName)
                    .ProjectTo<UsersExportServiceModel>(this.mapper.ConfigurationProvider)
                    .ToListAsync();

            return this.CreateReportFile(collection);
        }

        private async Task ValidateExportData(ExportDataServiceModel exportData)
        {
            var isExportTypeValid = exportData.ExportType != ExportType.Course.ToString() ||
                                    exportData.ExportType != ExportType.City.ToString();

            if (!isExportTypeValid)
            {
                throw new ArgumentException($"{exportData.ExportType} is not a valid type!");
            }

            var isCourseExist = exportData.ExportType == ExportType.Course.ToString() &&
                                await this.courseService.IsExistAsync(exportData.EntityToExportId);

            var isCityExist = exportData.ExportType == ExportType.City.ToString() &&
                              await this.cityService.IsExistAsync(exportData.EntityToExportId);

            if (!isCourseExist && !isCityExist)
            {
                throw new ArgumentException($"Invalid entity to export!");
            }
        }

        private (byte[] data, string contentType, string fileName) CreateReportFile<TEntity>(
            IEnumerable<TEntity> collection)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("Report");
            var range = worksheet.Cells["A1"].LoadFromCollection(collection, true);
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
