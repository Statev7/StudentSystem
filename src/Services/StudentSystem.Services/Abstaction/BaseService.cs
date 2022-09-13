namespace StudentSystem.Services.Abstaction
{
    using AutoMapper;

    using StudentSystem.Web.Data;

    public abstract class BaseService
    {

        protected BaseService(StudentSystemDbContext dbContext, IMapper mapper)
        {
            this.DbContext = dbContext;
            this.Mapper = mapper;
        }

        protected StudentSystemDbContext DbContext { get; }

        protected IMapper Mapper { get; }
    }
}
