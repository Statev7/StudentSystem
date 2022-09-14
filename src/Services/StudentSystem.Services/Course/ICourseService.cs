namespace StudentSystem.Services.Course
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentSystem.ViewModels.Course;

    public interface ICourseService
    {
        IEnumerable<AllCoursesViewModel> GetAll();

        TEntity GetById<TEntity>(int id);

        Task CreateAsync(CreateCourseBindingModel course);

        Task<bool> UpdateAsync(UpdateCourseBindingModel course);

        Task<bool> DeleteAsync(int id);
    }
}
