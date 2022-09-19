namespace StudentSystem.ViewModels.Course
{
    using System.Collections.Generic;

    using StudentSystem.ViewModels.Lesson;

    public class DetailCourseViewModel
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public IEnumerable<LessonIdNameViewModel> Lessons { get; set; }
    }
}
