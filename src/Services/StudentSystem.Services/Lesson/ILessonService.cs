namespace StudentSystem.Services.Lesson
{
    using System.Threading.Tasks;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Lesson;

    public interface ILessonService : IBaseService, ICreateUpdateService
    {
        Task<PageLessonViewModel> GetAllLessonsPagedAsync(int[] filters, int currentPage, int lessonsPerPage);
    }
}
