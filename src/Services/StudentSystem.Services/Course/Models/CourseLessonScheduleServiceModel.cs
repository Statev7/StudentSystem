namespace StudentSystem.Services.Course.Models
{
    using System.Collections.Generic;

    using StudentSystem.Services.Lesson.Models;

    public class CourseLessonScheduleServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<LessonScheduleServiceModel> Lessons { get; set; }
    }
}
