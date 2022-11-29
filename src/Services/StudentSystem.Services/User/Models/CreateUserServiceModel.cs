namespace StudentSystem.Services.User.Models
{
    using System.ComponentModel.DataAnnotations;

    using static StudentSystem.Data.Common.Constants;

    public class CreateUserServiceModel : UpdateUserServiceModel
    {
        [Required]
        [MaxLength(PASSWORD_MAX_LENGTH)]
        [MinLength(PASSWORD_MIN_LENGTH)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
	}
}
