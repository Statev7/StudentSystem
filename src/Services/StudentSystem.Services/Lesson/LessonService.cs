namespace StudentSystem.Services.Lesson
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Abstaction;
    using StudentSystem.Services.Course;
    using StudentSystem.Services.Lesson.Models;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.ViewModels.Page;
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

        public async Task<PageLessonViewModel> GetAllLessonsPagedAsync(
            int[] coursesIds, 
            int currentPage, 
            int lessonsPerPage)
        {
            var lessons = await this
                .GetAllAsQueryable<LessonForPageServiceModel>()
                .OrderBy(x => x.Title)
                .ToListAsync();

            coursesIds = coursesIds
                .Distinct()
                .ToArray();

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

            var lessonsAsViewModel = new List<EntityForPageViewModel>();
            foreach (var lesson in lessons)
            {
                var mapped = this.Mapper.Map<EntityForPageViewModel>(lesson);

                lessonsAsViewModel.Add(mapped);
            }

            var courses = this.courseService
                .GetAllAsQueryable<CourseIdNameViewModel>()
                .OrderBy(x => x.Name)
                .ToList();

            var model = new PageLessonViewModel
            {
                Lessons = lessonsAsViewModel,
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