namespace StudentSystem.Services.Lesson.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.ViewModels.Course;

    using static StudentSystem.Data.Common.Constants;

    public class LessonFormServiceModel
    {
        [Required]
        [MaxLength(LESSON_TITLE_MAX_LENGTH)]
        [MinLength(LESSON_TITLE_MIN_LENGTH)]
        public string Title { get; set; }

        [Required]
        [MaxLength(LESSON_CONTENT_MAX_LENGTH)]
        [MinLength(LESSON_CONTENT_MIN_LENGTH)]
        public string Content { get; set; }

        [Required]
        public DateTime? Begining { get; set; }

        [Required]
        public DateTime? End { get; set; }

        public int CourseId { get; set; }

        public IEnumerable<CourseIdNameViewModel> Courses { get; set; }
    }
}
