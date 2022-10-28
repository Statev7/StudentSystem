namespace StudentSystem.Services.Home
{
    using System.Threading.Tasks;

    using StudentSystem.ViewModels.Home;

    public interface IHomeService
    {
        Task<HomeViewModel> GetInformationAsync(string userId);
    }
}
