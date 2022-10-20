namespace StudentSystem.ViewModels.Home
{
    using System.Collections.Generic;

    using StudentSystem.ViewModels.Course;
    using StudentSystem.ViewModels.Lesson;

    public class StudentAllResourcesViewModel
    {
        public IEnumerable<CourseIdNameViewModel> Courses { get; set; }

        public IEnumerable<LessonScheduleViewModel> Lessons { get; set; }
    }
}
