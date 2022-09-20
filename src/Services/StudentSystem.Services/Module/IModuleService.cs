namespace StudentSystem.Services.Module
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using StudentSystem.Data.Models.StudentSystem;

    public interface IModuleService
    {
        Task<bool> RegisterForCourseAsync(Course course, ClaimsPrincipal user);
    }
}
