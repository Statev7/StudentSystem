namespace StudentSystem.ViewModels.Lesson
{
    using System.Collections.Generic;

	using StudentSystem.ViewModels.Course;
	using StudentSystem.ViewModels.Page;

	public class PageLessonViewModel : PageViewModel
    {
		public IEnumerable<LessonForPageViewModel> Lessons { get; set; }

		public IEnumerable<CourseIdNameViewModel> Courses { get; set; }
	}
}
