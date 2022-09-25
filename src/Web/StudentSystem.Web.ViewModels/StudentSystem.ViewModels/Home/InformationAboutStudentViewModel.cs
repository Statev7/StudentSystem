namespace StudentSystem.ViewModels.Home
{
    using System.Collections.Generic;
    using StudentSystem.ViewModels.Course;

    public class InformationAboutStudentViewModel
    {
        public IEnumerable<CourseLessonScheduleViewModel> Courses { get; set; }
    }
}
