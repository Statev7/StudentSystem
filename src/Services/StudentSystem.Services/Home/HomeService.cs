﻿namespace StudentSystem.Services.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

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
        private const string CATEGORIES_KEY = "Categories";

        private readonly StudentSystemDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ICourseService courseService;
        private readonly IReviewService reviewService;
        private readonly ICategoryService categoryService;
        private readonly IMemoryCache memoryCache;

        public HomeService(
            StudentSystemDbContext dbContext,
            IMapper mapper,
            ICourseService courseService,
            IReviewService reviewService,
            ICategoryService categoryService,
            IMemoryCache memoryCache)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.courseService = courseService;
            this.reviewService = reviewService;
            this.categoryService = categoryService;
            this.memoryCache = memoryCache;
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

            var categories = (List<int>)this.memoryCache.Get(CATEGORIES_KEY);
            if (categories == null)
            {
                categories = new List<int>();

                var csharpCategoryId = await this.categoryService.GetIdByNameAsync("C#");
                var javaCategoryId = await this.categoryService.GetIdByNameAsync("Java");
                var javaScriptCategoryId = await this.categoryService.GetIdByNameAsync("JavaScript");
                var pythonCategoryId = await this.categoryService.GetIdByNameAsync("Python");

                //Using this for filtring.
                categories.Add(csharpCategoryId);
                categories.Add(javaCategoryId);
                categories.Add(javaScriptCategoryId);
                categories.Add(pythonCategoryId);

                this.memoryCache.Set(CATEGORIES_KEY, categories, TimeSpan.FromDays(1));
            }

            model.CoursesReviews.CategoriesIds = categories;

            var newestCourseAsServiceModel = await this.courseService
                .GetAllAsQueryable<OpenCourseSeviceModel>()
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedOn)
                .FirstOrDefaultAsync();

            var newestCourse = this.mapper.Map<OpenCourseViewModel>(newestCourseAsServiceModel);

            model.CoursesReviews.NewestCourse = newestCourse;

            var openCoursesAsServiceModels = await this.courseService
                .GetAllAsQueryable<OpenCourseSeviceModel>()
                .Where(c => !c.IsDeleted && c.StartDate > DateTime.UtcNow)
                .Take(4)
                .ToListAsync();

            var openCourses = ConvertCoursesToViewModel(openCoursesAsServiceModels);
            model.CoursesReviews.OpenCourses = openCourses;

            model.CoursesReviews.Reviews = await this.reviewService
                .GetAllAsQueryable<ReviewForHomeViewModel>()
                .Where(r => !r.IsDeleted)
                .OrderByDescending(r => r.CreatedOn)
                .Take(3)
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
                                          Title = l.Title,
                                          Begining = l.Begining,
                                          End = l.End,
                                          CourseId = l.CourseId,
                                          CourseName = us.Course.Name,
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

            var studentInformation = this.ConvertStudentInformationToViewModels(studentResources);

            return studentInformation;
        }

        private StudentAllResourcesViewModel ConvertStudentInformationToViewModels(
            StudentInformationServiceModel studentResources)
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

        private IEnumerable<OpenCourseViewModel> ConvertCoursesToViewModel(IEnumerable<OpenCourseSeviceModel> courses)
        {
            var openCourses = new List<OpenCourseViewModel>();
            foreach (var course in courses)
            {
                var mappedCourse = this.mapper.Map<OpenCourseViewModel>(course);
                openCourses.Add(mappedCourse);
            }

            return openCourses;
        }
    }
}
