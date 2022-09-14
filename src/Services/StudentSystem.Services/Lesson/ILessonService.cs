namespace StudentSystem.Services.Lesson
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Lesson;

    public interface ILessonService
    {
        IEnumerable<TEntity> GetAll<TEntity>();

        CreateLessonBindingModel GetViewModelForCreate();

        Task CreateAsync(CreateLessonBindingModel lesson);

        IEnumerable<CourseIdNameViewModel> GetAllCourses();
    }
}
