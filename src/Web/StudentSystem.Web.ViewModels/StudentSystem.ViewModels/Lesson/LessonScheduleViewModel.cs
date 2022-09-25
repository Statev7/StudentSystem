namespace StudentSystem.ViewModels.Lesson
{
    using System;

    public class LessonScheduleViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Begining { get; set; }

        public DateTime End { get; set; }
    }
}
