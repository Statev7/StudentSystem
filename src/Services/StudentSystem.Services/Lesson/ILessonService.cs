namespace StudentSystem.Services.Lesson
{
    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Lesson;

    public interface ILessonService : IBaseService
    {
        PageLessonViewModel Paging(int courseId, int currentPage, int lessonsPerPage);
    }
}
