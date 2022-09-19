namespace StudentSystem.Services.Module
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Abstaction;
    using StudentSystem.Web.Data;

    public class ModuleService : BaseService, IModuleService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ModuleService(
            StudentSystemDbContext dbContext, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager) 
            : base(dbContext, mapper)
        {
            this.userManager = userManager;
        }

        public async Task<bool> RegisterForCourseAsync(Course course, string userId)
        {
            var user = this.userManager.Users
                .Include(u => u.UserCourses)
                .ToListAsync()
                .Result
                .SingleOrDefault(u => u.Id == userId);

            var isUserInCourseAlready = user.UserCourses.Any(uc => uc.CourseId == course.Id);
            if (isUserInCourseAlready)
            {
                return false;
            }

            var userCourse = new UserCourse()
            {
                ApplicationUser = user,
                Course = course,
                CreatedOn = DateTime.UtcNow
            };

            course.UserCourses.Add(userCourse);
            await this.DbContext.SaveChangesAsync();

            return true;
        }
    }
}
