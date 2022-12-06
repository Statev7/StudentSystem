namespace StudentSystem.Services.Resource
{
    using System.Threading.Tasks;

    using StudentSystem.Services.Abstaction;
    using StudentSystem.Services.Contracts;
    using StudentSystem.ViewModels.Resource;

    public interface IResourceService : IBaseService, ICreateUpdateService
    {
        Task<PageResourceViewModel> GetAllResourcesPagedAsync(int currentPage, int resourcesPerPage);
    }
}