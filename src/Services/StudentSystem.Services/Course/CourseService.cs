namespace StudentSystem.Services.Course
{
    using System;
    using System.Linq;
    using System.Globalization;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using AutoMapper;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Web.Data;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.Web.Common;
    using StudentSystem.Web.Infrastructure.Extensions;

    public class CourseService : BaseService<Course>, ICourseService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public CourseService(
            StudentSystemDbContext dbContext,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
            : base(dbContext, mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<bool> RegisterForCourseAsync(int courseId, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (user.IsUserInCourseAlready(this.DbContext, courseId, userId))
            {
                return false;
            }

            var userCourse = new UserCourse()
            {
                ApplicationUserId = userId,
                CourseId = courseId,
                CreatedOn = DateTime.UtcNow
            };

            var userFromDb = this.DbContext.Users.Find(userId);

            if (!user.IsInRole(GlobalConstants.STUDENT_ROLE))
            {
                await userManager.AddToRoleAsync(userFromDb, GlobalConstants.STUDENT_ROLE);
            }

            await this.DbContext.AddAsync(userCourse);
            await this.DbContext.SaveChangesAsync();

            await this.signInManager.RefreshSignInAsync(userFromDb);

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
    }
}
