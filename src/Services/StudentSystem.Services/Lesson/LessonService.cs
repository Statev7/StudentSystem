namespace StudentSystem.Services.Lesson
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.Services.Course;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.Web.Data;
    using StudentSystem.Data.Models.StudentSystem;

    public class LessonService : BaseService, ILessonService
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

        public IEnumerable<TEntity> GetAll<TEntity>()
            => this.DbContext.Lessons
                .Where(l => !l.IsDeleted)
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToList();

        public CreateLessonBindingModel GetViewModelForCreate() 
            => new CreateLessonBindingModel()
            {
                Begining = null,
                End = null,
                Courses = this.GetAllCourses()
            };

        public IEnumerable<CourseIdNameViewModel> GetAllCourses()
            => this.courseService
                .GetAll<CourseIdNameViewModel>()
                .ToList();

        public async Task CreateAsync(CreateLessonBindingModel lesson)
        {
            //TODO: Add validation for course!
            var lessonToAdd = this.Mapper.Map<Lesson>(lesson);

            await this.DbContext.AddAsync(lessonToAdd);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
