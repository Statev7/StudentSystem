namespace StudentSystem.Services.Course
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Course;

    public interface ICourseService : IBaseService
    {
        Task<bool> RegisterForCourseAsync(int courseId, ClaimsPrincipal user);

        DetailCourseViewModel GetDetails(int id);
    }
}
