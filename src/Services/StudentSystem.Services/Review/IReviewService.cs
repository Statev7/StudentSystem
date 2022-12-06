namespace StudentSystem.Services.Review
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.Services.Contracts;

    public interface IReviewService : IBaseService, ICreateUpdateService
	{
        Task<bool> IsAuthorOrAdminAsync(int reviewId, ClaimsPrincipal claims);
    }
}
