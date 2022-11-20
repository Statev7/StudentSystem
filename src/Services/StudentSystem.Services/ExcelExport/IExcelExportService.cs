namespace StudentSystem.Services.ExcelExport
{
    using System.Threading.Tasks;

    public interface IExcelExportService
    {
        Task<(byte[] data, string contentType, string fileName)> ExportStudentsByCourseAsync(int courseId);
    }
}
