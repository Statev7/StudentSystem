namespace StudentSystem.Services.Resource
{
    using System.Linq;
    using System.Threading.Tasks;

    using StudentSystem.ViewModels.Resource;

    public interface IResourceService
    {
        IQueryable<TEntity> GetAll<TEntity>(bool withDeleted = false);

        Task CreateAsync(CreateResourceBindingModel resource);
    }
}
