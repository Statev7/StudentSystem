namespace StudentSystem.Services.Course
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.Services.Contracts;
    using StudentSystem.Services.Course.Models;
    using StudentSystem.ViewModels.Course;

    public interface ICourseService : IBaseService
    {
        Task<AllCoursesViewModel> GetAllCoursesPagedAsync(
            int[] categoriesIds, 
            int currentPage, 
            int coursesPerPage);

        Task CreateAsync(CourseFormServiceModel course);

        Task<bool> UpdateAsync(int id, CourseFormServiceModel course);

        Task<DetailCourseViewModel> GetDetailsAsync(int id);

        Task<bool> RegisterForCourseAsync(int courseId, ClaimsPrincipal user);
    }
}
