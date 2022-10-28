namespace StudentSystem.Services.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Services.Category;
    using StudentSystem.Services.Course;
    using StudentSystem.Services.Course.Models;
    using StudentSystem.Services.Home.Models;
    using StudentSystem.Services.Lesson.Models;
    using StudentSystem.Services.Review;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Home;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.ViewModels.Resource;
    using StudentSystem.ViewModels.Review;
    using StudentSystem.Web.Data;

    public class HomeService : IHomeService
    {
        private const int COURSES_TO_DISPLAY = 4;
        private const int REVIEWS_TO_DISPLAY = 3;

        private readonly StudentSystemDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ICourseService courseService;
        private readonly IReviewService reviewService;
        private readonly ICategoryService categoryService;

        public HomeService(
            StudentSystemDbContext dbContext,
            IMapper mapper,
            ICourseService courseService,
            IReviewService reviewService,
            ICategoryService categoryService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.courseService = courseService;
            this.reviewService = reviewService;
            this.categoryService = categoryService;
        }

        public async Task<HomeViewModel> GetInformationAsync(string userId)
        {
            //In CoursesReviews we store open courses, reviews and categoriesIds.
            var model = new HomeViewModel()
            {
                CoursesReviews = new HomeCoursesAndReviewsViewModel(),
            };

            if (userId != null)
            {
                model.StudentResources = await this.GetUserInformation(userId);
            }

            //TODO: Caching
            var csharpCategoryId = await this.categoryService.GetIdByNameAsync("C#");
            var javaCategoryId = await this.categoryService.GetIdByNameAsync("Java");
            var javaScriptCategoryId = await this.categoryService.GetIdByNameAsync("JavaScript");
            var pythonCategoryId = await this.categoryService.GetIdByNameAsync("Python");

            //Using this for filtring.
            model.CoursesReviews.CategoriesIds = new List<int>()
            {
                csharpCategoryId,
                javaCategoryId,
                javaScriptCategoryId,
                pythonCategoryId
            };

            model.CoursesReviews.OpenCourses = await this.courseService
                .GetAllAsQueryable<OpenCourseViewModel>()
                .Take(COURSES_TO_DISPLAY)
                .ToListAsync();

            model.CoursesReviews.Reviews = await this.reviewService
                .GetAllAsQueryable<ReviewForHomeViewModel>()
                .OrderByDescending(r => r.CreatedOn)
                .Take(REVIEWS_TO_DISPLAY)
                .ToListAsync();

            return model;
        }

        private async Task<StudentAllResourcesViewModel> GetUserInformation(string userId)
        {
            //Takes only the resources that have not expired 

            var studentResources = await this.dbContext
                    .Users
                    .Where(u => u.Id == userId)
                    .Select(u => new StudentInformationServiceModel
                    {
                        Courses = u.UserCourses
                            .Where(u => !u.Course.IsDeleted && u.Course.EndDate >= DateTime.UtcNow)
                            .Select(us => new CourseLessonScheduleServiceModel
                            {
                                Id = us.CourseId,
                                Name = us.Course.Name,
                                Lessons = us.Course.Lessons
                                      .Where(l => l.End >= DateTime.UtcNow)
                                      .Select(l => new LessonScheduleServiceModel
                                      {
                                          Id = l.Id,
                                          CourseName = us.Course.Name,
                                          Title = l.Title,
                                          Begining = l.Begining,
                                          End = l.End,
                                          Resources = l.Resources
                                            .Where(r => r.Lesson.End >= DateTime.UtcNow)
                                            .Select(r => new ResourceViewModel
                                            {
                                                Id = r.Id,
                                                Name = r.Name,
                                                URL = r.URL,
                                            })
                                            .ToList()
                                      })
                                      .ToList()
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

            var studentInformation = this.ConvertToViewModels(studentResources);

            return studentInformation;
        }

        private StudentAllResourcesViewModel ConvertToViewModels(StudentInformationServiceModel studentResources)
        {
            var coursesAsViewModels = new List<CourseIdNameViewModel>();
            var lessonsAsViewModels = new List<LessonScheduleViewModel>();

            foreach (var course in studentResources.Courses)
            {
                var mappedCourse = this.mapper.Map<CourseIdNameViewModel>(course);

                coursesAsViewModels.Add(mappedCourse);
            }

            foreach (var course in studentResources.Courses)
            {
                foreach (var lesson in course.Lessons)
                {
                    var mappedLesson = this.mapper.Map<LessonScheduleViewModel>(lesson);

                    lessonsAsViewModels.Add(mappedLesson);
                }
            }

            var studentInformation = new StudentAllResourcesViewModel
            {
                Courses = coursesAsViewModels,
                Lessons = lessonsAsViewModels.OrderBy(x => x.Begining),
            };

            return studentInformation;
        }
    }
}
