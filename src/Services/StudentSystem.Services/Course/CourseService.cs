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
    using StudentSystem.Web.Infrastructure.Extensions;
    using StudentSystem.ViewModels.Review;

    using static StudentSystem.Web.Common.GlobalConstants;

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

            if (user.IsUserInCourse(this.userManager, courseId, userId))
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

            if (!user.IsInRole(STUDENT_ROLE) && !user.IsInRole(ADMIN_ROLE))
            {
                await this.userManager.RemoveFromRoleAsync(userFromDb, USER_ROLE);
                await this.userManager.AddToRoleAsync(userFromDb, STUDENT_ROLE);
            }

            await this.DbContext.AddAsync(userCourse);
            await this.DbContext.SaveChangesAsync();

            await this.signInManager.RefreshSignInAsync(userFromDb);

            return true;
        }

        public DetailCourseViewModel GetDetails(int id)
        {
            var course = this.DbSet
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
                            .ToList(),
                    Reviews = c.Reviews
                        .Where(r => !r.IsDeleted)
                        .OrderByDescending(r => r.CreatedOn)
                        .Select(r => new ReviewViewModel
                        {
                            Id = r.Id,
                            Content = r.Content,
                            UserId = r.UserId,
                            Username = r.User.UserName
                        })
                })
                .FirstOrDefault();

            return course;
        }
    }
}
