namespace StudentSystem.Services.User.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.ViewModels.City;

    using static StudentSystem.Data.Common.Constants;

    public class UpdateUserServiceModel
    {
        [Required]
        [MaxLength(FIRST_NAME_MAX_LENGTH)]
        [MinLength(FIRST_NAME_MIN_LENGTH)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LAST_NAME_MAX_LENGTH)]
        [MinLength(LAST_NAME_MIN_LENGTH)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Image Url")]
        [Url]
        public string ImageUrl { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }

        public IEnumerable<CityIdNameViewModel> Cities { get; set; }
    }
}
