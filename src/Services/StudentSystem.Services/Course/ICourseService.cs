namespace StudentSystem.Services.Course
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentSystem.ViewModels.Course;

    public interface ICourseService
    {
        IEnumerable<AllCoursesViewModel> GetAll();

        Task CreateAsync(CreateCourseBindingModel course);
    }
}
