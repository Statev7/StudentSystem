namespace StudentSystem.Services.Course
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Web.Data;
    using Microsoft.AspNetCore.Mvc.Infrastructure;

    public class CourseService : BaseService, ICourseService
    {
        public CourseService(
            StudentSystemDbContext dbContext, 
            IMapper mapper,
            IActionContextAccessor actionContextAccessor)
            :base(dbContext, mapper, actionContextAccessor)
        {
        }

        public IEnumerable<AllCoursesViewModel> GetAll()
            => this.DbContext
                .Courses
                .Where(x => !x.IsDeleted)
                .ProjectTo<AllCoursesViewModel>(this.Mapper.ConfigurationProvider)
                .ToList();

        public TEntity GetById<TEntity>(int id)
        {
            var course = this.DbContext.Courses.Find(id);

            if (course == null)
            {
                //TODO: Change error key and message!
                this.AddModelError("Error", "Entity with this id does not exist");
            }

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
