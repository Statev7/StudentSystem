namespace StudentSystem.Services.Resource
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.Page;
    using StudentSystem.ViewModels.Resource;
    using StudentSystem.Web.Data;

    public class ResourceService : BaseService<Resource>, IResourceService
    {
        public ResourceService(StudentSystemDbContext dbContext, IMapper mapper) 
            : base(dbContext, mapper)
        {
        }

        public async Task<PageResourceViewModel> GetAllResourcesPagedAsync(int currentPage, int resourcesPerPage)
        {
            var resources = this
                .GetAllAsQueryable<EntityForPageViewModel>()
                .OrderBy(r => r.Name);

            var pagedResources = await this
                .Paging(resources, currentPage, resourcesPerPage)
                .ToListAsync();

            var model = new PageResourceViewModel
            {
                Resources = pagedResources,
                CurrentPage = currentPage,
                EntitiesPerPage = resourcesPerPage,
                TotalEntities = resources.Count(),
            };

            return model;
        }
    }
}