namespace StudentSystem.Services.Review.Models
{
    using System.ComponentModel.DataAnnotations;

    using static StudentSystem.Data.Common.Constants;

    public class ReviewFormServiceModel
	{
        [Required]
        [MaxLength(ReviewContentMaxLength)]
        public string Content { get; set; }
    }
}
