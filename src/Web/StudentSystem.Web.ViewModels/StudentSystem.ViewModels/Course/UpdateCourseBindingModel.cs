namespace StudentSystem.ViewModels.Course
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using StudentSystem.Web.Infrastructure.Attributes;

    using static StudentSystem.Data.Common.Constants;

    public class UpdateCourseBindingModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CourseNameMaxLength)]
        [MinLength(CourseNameMinLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CourseDescriptionMaxLength)]
        [MinLength(CourseDescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        [Url]
        [Display(Name = "Image Url")]
        public string ImageURL { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}
