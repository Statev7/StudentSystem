namespace StudentSystem.Services.Home
{
    using System;
    using System.Linq;

    using AutoMapper;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.Services.Course;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Home;
    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.Web.Data;

    public class HomeService : BaseService, IHomeService
    {
        private readonly ICourseService courseService;

        public HomeService(
            StudentSystemDbContext dbContext, 
            IMapper mapper,
            ICourseService courseService) 
            : base(dbContext, mapper)
        {
            this.courseService = courseService;
        }

        public InformationAboutStudentViewModel GetInformation(string userId) 
            => this.DbContext.Users
                .Where(x => x.Id == userId)
                .Select(x => new InformationAboutStudentViewModel
                {
                    Courses = x.UserCourses
                               .Where(us => us.Course.EndDate >= DateTime.UtcNow)
                               .Select(us => new CourseLessonScheduleViewModel
                               {
                                   Id = us.Id,
                                   Name = us.Course.Name,
                                   Lessons = us.Course.Lessons
                                   .Where(l => l.End >= DateTime.UtcNow)
                                   .OrderBy(l => l.Begining)
                                   .Select(l => new LessonScheduleViewModel
                                   {
                                       Id = l.Id,
                                       Title = l.Title,
                                       Begining = l.Begining,
                                       End = l.End
                                   })
                               })
                               .ToList(),
                    LatestCourses = this.courseService.GetTheLatestCourses().ToList()
                })
                .FirstOrDefault();
    }
}
