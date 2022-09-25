namespace StudentSystem.Services.Module
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Identity;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Abstaction;
    using StudentSystem.Web.Common;
    using StudentSystem.Web.Data;
    using StudentSystem.Web.Infrastructure.Extensions;

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

            if (!user.IsInRole(GlobalConstants.STUDENT_ROLE))
            {
                var userFromDb = this.DbContext.Users.Find(userId);
                await userManager.AddToRoleAsync(userFromDb, GlobalConstants.STUDENT_ROLE);
            }

            course.UserCourses.Add(userCourse);
            await this.DbContext.SaveChangesAsync();

            return true;
        }
    }
}
