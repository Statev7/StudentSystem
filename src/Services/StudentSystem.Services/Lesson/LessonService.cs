namespace StudentSystem.Services.Lesson
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Abstaction;
    using StudentSystem.Services.Course;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.Web.Data;

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

        public PageLessonViewModel GetAllLessonsPaged(int[] coursesIds, int currentPage, int lessonsPerPage)
        {
            var lessons = this
                .GetAllAsQueryable<LessonForPageViewModel>()
                .OrderBy(x => x.Title)
                .ToList();

            coursesIds = coursesIds.Distinct().ToArray();

            if (coursesIds.Any())
            {
                lessons = lessons
                    .Where(x => coursesIds.Contains(x.CourseId))
                    .ToList();
            }

            var totalLessons = lessons.Count;

            if (lessons.Any())
            {
                lessons = this
                    .Paging(lessons, currentPage, lessonsPerPage)
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
                Filters = coursesIds,
                CurrentPage = currentPage,
                EntitiesPerPage = lessonsPerPage,
                TotalEntities = totalLessons,
            };

            return model;
        }
    }
}
