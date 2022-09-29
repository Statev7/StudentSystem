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

    public class ModuleService : IModuleService
    {
        private readonly StudentSystemDbContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ModuleService(
            StudentSystemDbContext dbContext, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager) 
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<bool> RegisterForCourseAsync(Course course, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (user.IsUserInCourseAlready(this.dbContext, course.Id, userId))
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
                var userFromDb = this.dbContext.Users.Find(userId);
                await userManager.AddToRoleAsync(userFromDb, GlobalConstants.STUDENT_ROLE);
            }

            course.UserCourses.Add(userCourse);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
