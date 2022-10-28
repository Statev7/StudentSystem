namespace StudentSystem.Services.Resource
{
    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Abstaction;
    using StudentSystem.Web.Data;

    public class ResourceService : BaseService<Resource>, IResourceService
    {

        public ResourceService(StudentSystemDbContext dbContext, IMapper mapper) 
            : base(dbContext, mapper)
        {
        }
    }
}
