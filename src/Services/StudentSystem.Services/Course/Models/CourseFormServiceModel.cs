namespace StudentSystem.Services.Course.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.ViewModels.Category;
    using StudentSystem.Web.Infrastructure.Filters;

    using static StudentSystem.Data.Common.Constants;

    public class CourseFormServiceModel
    {
        [Required]
        [MaxLength(COURSE_NAME_MAX_LENGTH)]
        [MinLength(COURSE_NAME_MIN_LENGTH)]
        public string Name { get; set; }

        [Required]
        [MaxLength(COURSE_DESCRIPTION_MAX_LENGTH)]
        [MinLength(COURSE_DESCRIPTION_MIN_LENGTH)]
        public string Description { get; set; }

        [Required]
        [Url]
        [Display(Name = "Image Url")]
        public string ImageURL { get; set; }

        [Required]
        [Display(Name = "Categories")]
        public int[] CategoriesIds { get; set; }

        [Required]
        [DateLessThanToday(canBeToday: false)]
        [DateLessThan(nameof(EndDate))]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public ICollection<CategoryIdNameViewModel> Categories { get; set; }
    }
}
