namespace StudentSystem.Services.City
{
    using AutoMapper;

    using Data.Models.StudentSystem;
    using StudentSystem.Services.Abstaction;
    using StudentSystem.Web.Data;

    public class CityService : BaseService<City>, ICityService
    {
        public CityService(StudentSystemDbContext dbContext, IMapper mapper) 
            : base(dbContext, mapper)
        {
        }
    }
}
