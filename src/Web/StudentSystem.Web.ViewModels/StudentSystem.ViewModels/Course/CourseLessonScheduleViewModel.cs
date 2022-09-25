namespace StudentSystem.ViewModels.Course
{
    using System.Collections.Generic;

    using StudentSystem.ViewModels.Lesson;

    public class CourseLessonScheduleViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<LessonScheduleViewModel> Lessons { get; set; }
    }
}
