namespace StudentSystem.Services.Home
{
    using StudentSystem.ViewModels.Home;

    public interface IHomeService
    {
        StudentInformationViewModel GetInformation(string userId);
    }
}
