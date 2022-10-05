namespace StudentSystem.Services.Lesson
{
    using System.Linq;

    using AutoMapper;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.Web.Data;
    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Course;
    using StudentSystem.ViewModels.Course;

    public class LessonService : BaseService<Lesson>, ILessonService
    {
        private readonly ICourseService courseService;

        public LessonService(
            StudentSystemDbContext dbContext,
            IMapper mapper,
            ICourseService courseService)
            : base(dbContext, mapper)
        {
            this.courseService = courseService;
        }

        public PageLessonViewModel GetAllLessonsPaged(int courseId, int currentPage, int lessonsPerPage)
        {
            var query = this.PageingAsQueryable<LessonPagingViewModel>(currentPage, lessonsPerPage);

            if (courseId != 0)
            {
                query = query
                    .Where(x => x.CourseId == courseId);
            }

            var lessons = query.ToList();
            var model = new PageLessonViewModel
            {
                Lessons = lessons,
                Courses = this.courseService.GetAllAsQueryable<CourseIdNameViewModel>().ToList(),
                CourseId = courseId,
                CurrentPage = currentPage,
                LessonsPerPage = lessonsPerPage,
                TotalLessons = this.GetCountAsync().GetAwaiter().GetResult(),
            };

            return model;
        }
    }
}
