namespace StudentSystem.Services.Resource
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Resource;
    using StudentSystem.Web.Data;

    public class ResourceService : BaseService, IResourceService
    {

        public ResourceService(StudentSystemDbContext dbContext, IMapper mapper) 
            : base(dbContext, mapper)
        {
        }

        public IQueryable<TEntity> GetAll<TEntity>(bool withDeleted = false)
        {
            var quaery = this.DbContext.Resources.AsQueryable();

            if (!withDeleted)
            {
                quaery = quaery
                    .Where(x => !x.IsDeleted);
            }

            return quaery.ProjectTo<TEntity>(this.Mapper.ConfigurationProvider);
        }

        public async Task CreateAsync(CreateResourceBindingModel resource)
        {
            var resourceToAdd = this.Mapper.Map<Resource>(resource);

            resourceToAdd.CreatedOn = DateTime.UtcNow;

            await this.DbContext.AddAsync(resourceToAdd);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
