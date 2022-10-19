namespace StudentSystem.Data.Models.StudentSystem
{
    using System.ComponentModel.DataAnnotations;

    using Data.Models.Abstraction;

    using static Data.Common.Constants;

    public class Review : BaseModel
    {
        [Required]
        [MaxLength(REVIEW_CONTENT_MAX_LENGTH)]
        public string Content { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
