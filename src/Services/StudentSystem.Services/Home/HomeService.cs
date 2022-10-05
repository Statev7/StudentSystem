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
    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Home;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.Web.Data;

    public class HomeService : IHomeService
    {
        private readonly StudentSystemDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ICourseService courseService;

        public HomeService(
            StudentSystemDbContext dbContext, 
            IMapper mapper,
            ICourseService courseService) 
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.courseService = courseService;
        }

        public HomeViewModel GetInformation(string userId)
        {
            if (userId == null)
            {
                return new HomeViewModel();
            }

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

            var model = this.ConvertToViewModels(studentResources);
            model.OpenCourses = this.courseService
                .GetAllAsQueryable<CourseViewModel>()
                .Where(x => x.StartDate > DateTime.UtcNow)
                .OrderBy(x => x.StartDate)
                .Take(3)
                .ToList();

            return model;
        }

        private HomeViewModel ConvertToViewModels(StudentInformationServiceModel studentResources)
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

            var studentInformation = new HomeViewModel
            {
                Courses = coursesAsViewModels,
                Lessons = lessonsAsViewModels.OrderBy(x => x.Begining),
            };

            return studentInformation;
        }
    }
}
