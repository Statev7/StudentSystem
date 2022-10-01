namespace StudentSystem.ViewModels.Review
{
    using System.ComponentModel.DataAnnotations;

    using static StudentSystem.Data.Common.Constants;

    public class CreateReviewBindingModel
    {
        [Required]
        [MaxLength(ReviewContentMaxLength)]
        public string Content { get; set; }

        public int CourseId { get; set; }
    }
}
