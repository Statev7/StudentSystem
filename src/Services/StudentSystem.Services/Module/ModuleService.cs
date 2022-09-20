namespace StudentSystem.Services.Module
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Abstaction;
    using StudentSystem.Web.Data;
    using StudentSystem.Web.Infrastructure.Extensions;

    public class ModuleService : BaseService, IModuleService
    {
        public ModuleService(
            StudentSystemDbContext dbContext, 
            IMapper mapper) 
            : base(dbContext, mapper)
        {
        }

        public async Task<bool> RegisterForCourseAsync(Course course, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (user.IsUserInCourseAlready(this.DbContext, course.Id, userId))
            {
                return false;
            }

            var userCourse = new UserCourse()
            {
                ApplicationUserId = userId,
                Course = course,
                CreatedOn = DateTime.UtcNow
            };

            course.UserCourses.Add(userCourse);
            await this.DbContext.SaveChangesAsync();

            return true;
        }
    }
}
