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
    using System.Collections.Generic;

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
            //Get all lessons as queryable
            var queryForPageing = this
                .GetAllAsQueryable<LessonForPageViewModel>()
                .OrderBy(x => x.Title)
                .AsQueryable();

            //Filtered them if needed
            if (courseId != 0)
            {
                queryForPageing = queryForPageing
                    .Where(x => x.CourseId == courseId);
            }

            var lessons = new List<LessonForPageViewModel>();
            var totalEntities = 0;

            if (queryForPageing.Any())
            {
                totalEntities = queryForPageing.Count();

                lessons = this
                    .PageingAsQueryable(queryForPageing, currentPage, lessonsPerPage)
                    .ToList();
            }

            var courses = this.courseService
                .GetAllAsQueryable<CourseIdNameViewModel>()
                .OrderBy(x => x.Name)
                .ToList();

            var model = new PageLessonViewModel
            {
                Lessons = lessons,
                Courses = courses,
                CourseId = courseId,
                CurrentPage = currentPage,
                EntitiesPerPage = lessonsPerPage,
                TotalEntities = totalEntities,
            };

            return model;
        }
    }
}
