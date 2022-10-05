namespace StudentSystem.Services.Home
{
    using StudentSystem.ViewModels.Home;

    public interface IHomeService
    {
        HomeViewModel GetInformation(string userId);
    }
}
