namespace StudentSystem.Services.Lesson
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.Web.Data;
    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Course;
    using StudentSystem.ViewModels.Course;

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

        public IQueryable<TEntity> GetAll<TEntity>(bool withDeleted = false)
        {
            var query = this.DbContext.Lessons.AsQueryable();

            if (!withDeleted)
            {
                query = query.Where(x => !x.IsDeleted);
            }

            return query.ProjectTo<TEntity>(this.Mapper.ConfigurationProvider);
        }


        public TEntity GetById<TEntity>(int id)
        {
            var lesson = this.DbContext.Lessons.Find(id);

            var lessonToReturn = this.Mapper.Map<TEntity>(lesson);
            return lessonToReturn;
        }

        public PageLessonViewModel Paging(int courseId, int currentPage, int lessonsPerPage)
        {
            var query = this.GetAll<LessonPagingViewModel>();

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
                Courses = this.courseService.GetAll<CourseIdNameViewModel>(),
                CourseId = courseId,
                CurrentPage = currentPage,
                LessonsPerPage = lessonsPerPage
            };

            return model;
        }

        public async Task CreateAsync(CreateLessonBindingModel lesson)
        {
            var lessonToAdd = this.Mapper.Map<Lesson>(lesson);

            await this.DbContext.AddAsync(lessonToAdd);
            await this.DbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateLessonBindingModel lesson)
        {
            var lessonToUpdate = this.GetById<Lesson>(lesson.Id);

            if (lessonToUpdate == null)
            {
                return false;
            }

            this.Mapper.Map(lesson, lessonToUpdate);
            lessonToUpdate.ModifiedOn = DateTime.UtcNow;

            this.DbContext.Lessons.Update(lessonToUpdate);
            await this.DbContext.SaveChangesAsync();

            return true;
        }
    }
}
