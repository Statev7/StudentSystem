namespace StudentSystem.Services.Administrator
{
    using System.Threading.Tasks;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.ViewModels.User;

    public interface IAdministratorService
    {
        UserPageViewModel GetUsers(string search, int currentPage);

        Task<bool> PromoteAsync(ApplicationUser user);

        Task<bool> DemoteAsync(ApplicationUser user);

        Task<bool> BanAsync(string userId);

        Task<bool> UnbanAsync(string userId);

        Task<bool> IsUserBannedAsync(string userId);

        Task<bool> IsUserExistAsync(string id);
    }
}
