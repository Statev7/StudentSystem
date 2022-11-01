namespace StudentSystem.ViewModels.Course
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.ViewModels.Lesson;
    using StudentSystem.ViewModels.Review;

    using static StudentSystem.Data.Common.Constants;

    public class DetailCourseViewModel
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public IEnumerable<LessonDetailsViewModel> Lessons { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }

        [Required]
        [MaxLength(REVIEW_CONTENT_MAX_LENGTH)]
        public string Content { get; set; }
    }
}
