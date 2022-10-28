namespace StudentSystem.ViewModels.Lesson
{
    using System;
    using System.Collections.Generic;

    using StudentSystem.ViewModels.Resource;

    public class LessonScheduleViewModel
    {
        public int Id { get; set; }

        public string CourseName { get; set; }

        public string Title { get; set; }

        public DateTime Begining { get; set; }

        public DateTime End { get; set; }

        public IEnumerable<ResourceViewModel> Resources { get; set; }
    }
}
