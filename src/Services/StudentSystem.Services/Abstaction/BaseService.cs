namespace StudentSystem.Services.Abstaction
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc.Infrastructure;

    using StudentSystem.Web.Data;

    public abstract class BaseService
    {
        private readonly IActionContextAccessor actionContextAccessor;

        protected BaseService(StudentSystemDbContext dbContext, IMapper mapper)
        {
            this.DbContext = dbContext;
            this.Mapper = mapper;
        }

        protected BaseService(
            StudentSystemDbContext dbContext, 
            IMapper mapper, 
            IActionContextAccessor actionContextAccessor)
            :this(dbContext, mapper)
        {
            this.actionContextAccessor = actionContextAccessor;
        }

        protected StudentSystemDbContext DbContext { get; }

        protected IMapper Mapper { get; }

        protected void AddModelError(string key, string errorMessage) 
            => this.actionContextAccessor.ActionContext.ModelState.AddModelError(key, errorMessage);
    }
}
