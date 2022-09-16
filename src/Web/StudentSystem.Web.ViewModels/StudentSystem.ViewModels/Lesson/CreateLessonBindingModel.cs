﻿namespace StudentSystem.ViewModels.Lesson
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.ViewModels.Course;

    using static Data.Common.Constants;

    public class CreateLessonBindingModel
	{
        [Required]
        [MaxLength(LessonTitleMaxLength)]
        [MinLength(LessonTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(LessonContentMaxLength)]
        [MinLength(LessonContentMinLength)]
        public string Content { get; set; }

        [Required]
        public DateTime? Begining { get; set; }

        [Required]
        public DateTime? End { get; set; }

        public int CourseId { get; set; }

        public IEnumerable<CourseIdNameViewModel> Courses { get; set; }
    }
}