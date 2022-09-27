namespace StudentSystem.Services.Course
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Web.Data;
    using StudentSystem.ViewModels.Lesson;
    using System.Globalization;

    public class CourseService : BaseService, ICourseService
    {
        public CourseService(
            StudentSystemDbContext dbContext,
            IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public IQueryable<TEntity> GetAll<TEntity>(bool withDeleted = false)
        {
            var quaery = this.DbContext.Courses.AsQueryable();

            if (!withDeleted)
            {
                quaery = quaery
                    .Where(x => !x.IsDeleted);
            }

            return quaery.ProjectTo<TEntity>(this.Mapper.ConfigurationProvider);
        }

        public TEntity GetById<TEntity>(int id)
        {
            var course = this.DbContext
                .Courses
                .Where(c => !c.IsDeleted)
                .SingleOrDefault(c => c.Id == id);

            var courseToReturn = this.Mapper.Map<TEntity>(course);
            return courseToReturn;
        }

        public async Task CreateAsync(CreateCourseBindingModel course)
        {
            var courseToAdd = this.Mapper.Map<Course>(course);

            courseToAdd.CreatedOn = DateTime.UtcNow;

            await this.DbContext.Courses.AddAsync(courseToAdd);
            await this.DbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateCourseBindingModel course)
        {
            var courseToUpdate = this.DbContext.Courses.Find(course.Id);

            if (courseToUpdate == null)
            {
                return false;
            }

            this.Mapper.Map(course, courseToUpdate);
            courseToUpdate.ModifiedOn = DateTime.UtcNow;

            this.DbContext.Update(courseToUpdate);
            await this.DbContext.SaveChangesAsync();

            return true;
        }

        public DetailCourseViewModel GetDetails(int id)
        {
            var course = this.DbContext.Courses
                .Where(x => x.Id == id)
                .Select(c => new DetailCourseViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    StartDate = c.StartDate.ToString("d MMMM yyyy", CultureInfo.InvariantCulture),
                    EndDate = c.EndDate.ToString("d MMMM yyyy", CultureInfo.InvariantCulture),
                    Lessons = c.Lessons
                            .Where(l => !l.IsDeleted)
                            .Select(l => new LessonIdNameViewModel
                            {
                                Id = l.Id,
                                Title = l.Title,
                            })
                            .ToList()
                })
                .FirstOrDefault();

            return course;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = this.GetById<Course>(id);

            if (course == null)
            {
                return false;
            }

            course.IsDeleted = true;
            course.DeletedOn = DateTime.UtcNow;

            this.DbContext.Update(course);
            await this.DbContext.SaveChangesAsync();

            return true;
        }
    }
}
