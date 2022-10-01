namespace StudentSystem.ViewModels.Course
{
    using System.Collections.Generic;

    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.ViewModels.Review;

    public class DetailCourseViewModel
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public IEnumerable<LessonIdNameViewModel> Lessons { get; set; }
        
        public CreateReviewBindingModel CreateReviewModel { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}
