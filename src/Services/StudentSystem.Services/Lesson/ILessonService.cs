namespace StudentSystem.Services.Lesson
{
    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Lesson;

    public interface ILessonService : IBaseService
    {
        PageLessonViewModel GetAllLessonsPaged(int[] filters, int currentPage, int lessonsPerPage);
    }
}
