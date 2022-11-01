namespace StudentSystem.ViewModels.Lesson
{
    using System;
    using System.Collections.Generic;
    using StudentSystem.ViewModels.Resource;

    public class LessonDetailsViewModel
	{
		public int Id { get; set; }

		public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Begining { get; set; }

        public DateTime End { get; set; }

        public IEnumerable<ResourceIdNameViewModel> Resources { get; set; }
    }
}
