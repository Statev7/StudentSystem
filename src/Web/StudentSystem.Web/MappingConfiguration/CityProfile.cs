namespace StudentSystem.Web.MappingConfiguration
{
    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.ViewModels.City;

    public class CityProfile : Profile
    {
        public CityProfile()
        {
            this.CreateMap<City, CityIdNameViewModel>();
        }
    }
}
