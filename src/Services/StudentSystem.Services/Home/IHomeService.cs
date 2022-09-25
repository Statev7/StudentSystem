namespace StudentSystem.Services.Home
{
    using StudentSystem.ViewModels.Home;

    public interface IHomeService
    {
        InformationAboutStudentViewModel GetInformation(string userId);
    }
}
