namespace StudentSystem.Services.Lesson
{
    using System;
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

        public PageLessonViewModel Paging(int courseId, int currentPage, int lessonsPerPage)
        {
            var query = this.GetAllAsQueryable<LessonPagingViewModel>();

            var totalPages = Math.Ceiling((double)query.Count() / lessonsPerPage);

            if (currentPage < 1)
            {
                currentPage = 1;
            }
            else if(currentPage > totalPages)
            {
                currentPage = (int)totalPages;
            }

            if (courseId != 0)
            {
                query = query.Where(x => x.CourseId == courseId);
            }

            var lessons = query
                .Skip((currentPage - 1) * lessonsPerPage)
                .Take(lessonsPerPage)
                .ToList();


            var model = new PageLessonViewModel
            {
                Lessons = lessons,
                Courses = this.courseService.GetAllAsQueryable<CourseIdNameViewModel>().ToList(),
                CourseId = courseId,
                CurrentPage = currentPage,
                LessonsPerPage = lessonsPerPage
            };

            return model;
        }
    }
}
