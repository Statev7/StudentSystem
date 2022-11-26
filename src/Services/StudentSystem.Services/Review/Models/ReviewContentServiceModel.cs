namespace StudentSystem.Services.Review.Models
{
    using System.ComponentModel.DataAnnotations;

    using static StudentSystem.Data.Common.Constants;

    public class ReviewContentServiceModel
	{
		[Required]
        [MaxLength(REVIEW_CONTENT_MAX_LENGTH)]
        public string Content { get; set; }
	}
}
