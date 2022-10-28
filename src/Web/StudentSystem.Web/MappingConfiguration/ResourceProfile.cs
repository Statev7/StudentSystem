namespace StudentSystem.Web.MappingConfiguration
{
    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Resource.Models;
    using StudentSystem.ViewModels.Resource;

    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            this.CreateMap<Resource, ResourceFormServiceModel>().ReverseMap();
            this.CreateMap<Resource, ResourceViewModel>();
        }
    }
}
