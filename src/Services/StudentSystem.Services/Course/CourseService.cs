namespace StudentSystem.Services.Course
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Abstaction;
    using StudentSystem.Services.Category;
    using StudentSystem.Services.Course.Models;
    using StudentSystem.ViewModels.Category;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.ViewModels.Review;
    using StudentSystem.Web.Data;
    using StudentSystem.Web.Infrastructure.Extensions;

    using static StudentSystem.Web.Common.GlobalConstants;

    public class CourseService : BaseService<Course>, ICourseService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ICategoryService categoryService;

        public CourseService(
            StudentSystemDbContext dbContext,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICategoryService categoryService)
            : base(dbContext, mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.categoryService = categoryService;
        }

        public async Task<AllCoursesViewModel> GetAllCoursesPagedAsync(
            int[] categoriesIds, 
            int currentPage, 
            int coursesPerPage)
        {
            categoriesIds = categoriesIds
                .Distinct()
                .ToArray();

            var courses = await this
                .GetAllAsQueryable<CourseViewModel>()
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Name)
                .ToListAsync();

            if (categoriesIds.Any())
            {
                courses = courses
                    .Where(c => c.CategoriesIds.Intersect(categoriesIds).Any())
                    .ToList();
            }

            var totalCourses = courses.Count;

            if (courses.Any())
            {
                courses = this
                    .Paging(courses, currentPage, coursesPerPage)
                    .ToList();
            }

            var categories = await this
                .categoryService
                .GetAllAsync<CategoryIdNameViewModel>();

            var model = new AllCoursesViewModel()
            {
                Courses = courses,
                Categories = categories,
                Filters = categoriesIds,
                CurrentPage = currentPage,
                EntitiesPerPage = coursesPerPage,
                TotalEntities = totalCourses,
            };

            return model;
        }

        public async Task CreateAsync(CourseFormServiceModel course)
        {
            var courseToCreate = this.Mapper.Map<Course>(course);
            courseToCreate.CreatedOn = DateTime.UtcNow;

            var categories = new List<CourseCategory>();

            foreach (var categoryId in course.CategoriesIds.Distinct())
            {
                var courseCategory = new CourseCategory()
                {
                    CategoryId = categoryId,
                    CourseId = courseToCreate.Id,
                    CreatedOn = DateTime.UtcNow,
                };

                categories.Add(courseCategory);
            }

            courseToCreate.CourseCategories = categories;

            await this.DbContext.Courses.AddAsync(courseToCreate);
            await this.DbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, CourseFormServiceModel course)
        {
            var courseToUpdate = await this.DbSet
                .Include(x => x.CourseCategories)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (courseToUpdate == null)
            {
                return false;
            }

            var categories = new List<CourseCategory>();
            foreach (var categoryId in course.CategoriesIds.Distinct())
            {
                var courseCategory = new CourseCategory()
                {
                    CategoryId = categoryId,
                    CourseId = courseToUpdate.Id,
                    CreatedOn = DateTime.UtcNow,
                };

                categories.Add(courseCategory);
            }

            this.Mapper.Map(course, courseToUpdate);

            courseToUpdate.CourseCategories = categories;
            courseToUpdate.ModifiedOn = DateTime.UtcNow;

            this.DbSet.Update(courseToUpdate);
            await this.DbContext.SaveChangesAsync();

            return true;
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

        public async Task<DetailCourseViewModel> GetDetailsAsync(int id)
        {
            var course = await this.DbSet
                .Where(x => x.Id == id && !x.IsDeleted)
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
                            Username = r.User.UserName,
                            UserImageIRL = r.User.ImageURL
                        })
                })
                .FirstOrDefaultAsync();

            return course;
        }
    }
}
