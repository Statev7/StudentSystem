namespace StudentSystem.Services.Review.Models
{
	public class CreateReviewServiceModel
	{
		public int CourseId { get; set; }

		public string UserId { get; set; }

		public string Content { get; set; }
	}
}
