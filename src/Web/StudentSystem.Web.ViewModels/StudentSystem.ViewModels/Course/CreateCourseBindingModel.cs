namespace StudentSystem.ViewModels.Course
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static StudentSystem.Data.Common.Constants;

    public class CreateCourseBindingModel
    {
        [Required]
        [MaxLength(CourseNameMaxLength)]
        [MinLength(CourseNameMinLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CourseDescriptionMaxLength)]
        [MinLength(CourseDescriptionMinLength)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
