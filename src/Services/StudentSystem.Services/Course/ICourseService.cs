namespace StudentSystem.Services.Course
{
    using System.Linq;
    using System.Threading.Tasks;

    using StudentSystem.ViewModels.Course;

    public interface ICourseService
    {
        IQueryable<TEntity> GetAll<TEntity>(bool withDeleted = false);

        TEntity GetById<TEntity>(int id);

        Task CreateAsync(CreateCourseBindingModel course);

        Task<bool> UpdateAsync(UpdateCourseBindingModel course);

        DetailCourseViewModel GetDetails(int id);

        Task<bool> DeleteAsync(int id);
    }
}
