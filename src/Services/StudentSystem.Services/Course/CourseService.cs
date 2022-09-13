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

    public class CourseService : BaseService, ICourseService
    {
        public CourseService(StudentSystemDbContext dbContext, IMapper mapper)
            :base(dbContext, mapper)
        {
        }

        public IEnumerable<AllCoursesViewModel> GetAll()
            => this.DbContext
                .Courses
                .ProjectTo<AllCoursesViewModel>(this.Mapper.ConfigurationProvider)
                .ToList();


        public async Task CreateAsync(CreateCourseBindingModel course)
        {
            var courseToAdd = this.Mapper.Map<Course>(course);

            courseToAdd.CreatedOn = DateTime.UtcNow;

            await this.DbContext.Courses.AddAsync(courseToAdd);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
