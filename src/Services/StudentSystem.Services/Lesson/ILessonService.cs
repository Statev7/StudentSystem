namespace StudentSystem.Services.Lesson
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentSystem.ViewModels.Lesson;

    public interface ILessonService
    {
        IEnumerable<TEntity> GetAll<TEntity>(bool withDeleted = false);

        TEntity GetById<TEntity>(int id);

        Task CreateAsync(CreateLessonBindingModel lesson);
    }
}
