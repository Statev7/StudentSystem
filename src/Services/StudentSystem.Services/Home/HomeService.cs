namespace StudentSystem.Services.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using StudentSystem.Services.Course;
    using StudentSystem.Services.Course.Models;
    using StudentSystem.Services.Home.Models;
    using StudentSystem.Services.Lesson.Models;
    using StudentSystem.Services.Review;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Home;
    using StudentSystem.ViewModels.Lesson;
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

        public HomeService(
            StudentSystemDbContext dbContext,
            IMapper mapper,
            ICourseService courseService,
            IReviewService reviewService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.courseService = courseService;
            this.reviewService = reviewService;
        }

        public HomeViewModel GetInformation(string userId)
        {
            var model = new HomeViewModel()
            {
                CoursesReviews = new HomeCoursesAndReviewsViewModel(),
            };

            if (userId != null)
            {
                model.StudentResources = this.GetUserInformation(userId);
            }

            model.CoursesReviews.OpenCourses = this.courseService
                .GetAllAsQueryable<OpenCourseViewModel>()
                .Where(x => x.StartDate >= DateTime.UtcNow)
                .Take(COURSES_TO_DISPLAY)
                .ToList();

            model.CoursesReviews.Reviews = this.reviewService
                .GetAllAsQueryable<ReviewForHomeViewModel>()
                .OrderByDescending(r => r.CreatedOn)
                .Take(REVIEWS_TO_DISPLAY)
                .ToList();

            return model;
        }

        private StudentAllResourcesViewModel GetUserInformation(string userId)
        {
            var studentResources = this.dbContext
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
                                          End = l.End
                                      })
                                      .ToList()
                        })
                    })
                    .FirstOrDefault();

            var studentInformation = this.ConvertToViewModels(studentResources);

            return studentInformation;
        }

        private StudentAllResourcesViewModel ConvertToViewModels(StudentInformationServiceModel studentResources)
        {
            var coursesAsViewModels = new List<CourseIdNameViewModel>();
            var lessonsAsViewModels = new List<LessonScheduleViewModel>();

            foreach (var course in studentResources.Courses)
            {
                var mapped = this.mapper.Map<CourseIdNameViewModel>(course);

                coursesAsViewModels.Add(mapped);
            }

            foreach (var course in studentResources.Courses)
            {
                foreach (var lesson in course.Lessons)
                {
                    var mapped = this.mapper.Map<LessonScheduleViewModel>(lesson);

                    lessonsAsViewModels.Add(mapped);
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
