namespace StudentSystem.Services.Administrator
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.ViewModels.User;

    public interface IAdministratorService
    {
        IEnumerable<UserViewModel> GetAll();

        Task<bool> PromoteAsync(ApplicationUser user);

        Task<bool> DemoteAsync(ApplicationUser user);
    }
}
