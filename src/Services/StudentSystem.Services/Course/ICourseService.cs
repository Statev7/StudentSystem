namespace StudentSystem.Services.Course
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentSystem.ViewModels.Course;

    public interface ICourseService
    {
        IEnumerable<TEntity> GetAll<TEntity>();

        TEntity GetById<TEntity>(int id);

        Task CreateAsync(CreateCourseBindingModel course);

        Task<bool> UpdateAsync(UpdateCourseBindingModel course);

        DetailCourseViewModel GetDetails(int id);

        Task<bool> DeleteAsync(int id);
    }
}
