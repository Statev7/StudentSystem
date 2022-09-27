namespace StudentSystem.Services.Lesson.Models
{
    using System;

    public class LessonScheduleServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Begining { get; set; }

        public DateTime End { get; set; }
    }
}
