namespace StudentSystem.ViewModels.Lesson
{
    using System.Collections.Generic;

	using StudentSystem.ViewModels.Course;

	public class PageLessonViewModel
	{
		public IEnumerable<LessonPagingViewModel> Lessons { get; set; }

		public IEnumerable<CourseIdNameViewModel> Courses { get; set; }

		public int CourseId { get; set; }

		public int CurrentPage { get; set; } = 1;

		public int LessonsPerPage { get; set; }

		public int TotalLessons { get; set; }
	}
}
