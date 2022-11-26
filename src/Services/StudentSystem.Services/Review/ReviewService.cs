namespace StudentSystem.Services.Review
{
	using System.Linq;
	using System.Security.Claims;
	using System.Threading.Tasks;

	using AutoMapper;

	using Microsoft.EntityFrameworkCore;

	using StudentSystem.Data.Models.StudentSystem;
	using StudentSystem.Services.Abstaction;
	using StudentSystem.Web.Data;
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
			var reviewAuthorId = await this.DbSet
					.Where(r => r.Id == reviewId)
					.Select(x => x.UserId)
					.FirstOrDefaultAsync();

            var hasPermission = reviewAuthorId == claims.GetId() || 
								claims.IsAdministrator();

			return hasPermission;
        }
	}
}
