namespace StudentSystem.Services.Review
{
	using AutoMapper;

	using StudentSystem.Services.Abstaction;
	using StudentSystem.Web.Data;
	using StudentSystem.Data.Models.StudentSystem;

	public class ReviewService : BaseService<Review>, IReviewService
	{
		public ReviewService(
			StudentSystemDbContext dbContext, 
			IMapper mapper) 
			: base(dbContext, mapper)
		{
		}
	}
}
