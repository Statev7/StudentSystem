namespace StudentSystem.Services.Review
{
	using AutoMapper;

	using StudentSystem.Services.Abstaction;
	using StudentSystem.Web.Data;
	using StudentSystem.Data.Models.StudentSystem;
	using System.Threading.Tasks;
	using StudentSystem.Services.Review.Models;
	using System.Security.Claims;
	using StudentSystem.Web.Infrastructure.Extensions;

	public class ReviewService : BaseService<Review>, IReviewService
	{
		public ReviewService(
			StudentSystemDbContext dbContext, 
			IMapper mapper) 
			: base(dbContext, mapper)
		{
		}

		public async Task<bool> IsAuthorOrAdminAsync(int reviewId, ClaimsPrincipal claims)
		{
            var review = await this.GetByIdAsync<ReviewUserIdServiceModel>(reviewId);

            var isAuthorOrAdmin = review.UserId == claims.GetId() || claims.IsAdministrator();
			return isAuthorOrAdmin;
        }
	}
}
