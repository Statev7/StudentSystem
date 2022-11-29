namespace StudentSystem.Services.User.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserLoginServiceModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
