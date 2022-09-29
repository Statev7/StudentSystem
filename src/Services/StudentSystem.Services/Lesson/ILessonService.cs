namespace StudentSystem.Services.Lesson
{
    using System.Linq;
    using System.Threading.Tasks;

    using StudentSystem.ViewModels.Lesson;

    public interface ILessonService
    {
        IQueryable<TEntity> GetAll<TEntity>(bool withDeleted = false);

        TEntity GetById<TEntity>(int id);

        PageLessonViewModel Paging(int courseId, int currentPage, int lessonsPerPage);

        Task CreateAsync(CreateLessonBindingModel lesson);

        Task<bool> UpdateAsync(UpdateLessonBindingModel lesson);
    }
}
