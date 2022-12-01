namespace StudentSystem.Services.ExcelExport
{
    using System.Threading.Tasks;
    using StudentSystem.Services.ExcelExport.Models;

    public interface IExcelExportService
    {
        Task<(byte[] data, string contentType, string fileName)> ExportAsync(ExportDataServiceModel exportData);
    }
}
