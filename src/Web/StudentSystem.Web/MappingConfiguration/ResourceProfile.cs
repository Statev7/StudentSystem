namespace StudentSystem.Web.MappingConfiguration
{
    using AutoMapper;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Resource.Models;
    using StudentSystem.ViewModels.Page;
    using StudentSystem.ViewModels.Resource;

    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            this.CreateMap<Resource, ResourceFormServiceModel>().ReverseMap();
            this.CreateMap<Resource, ResourceViewModel>();
            this.CreateMap<Resource, EntityForPageViewModel>()
                .ForMember(d => d.Name, conf => conf.MapFrom(s => s.Name));
            this.CreateMap<Resource, ResourceDetailsServiceModel>()
                .ForMember(d => d.CourseId, conf => conf.MapFrom(s => s.Lesson.CourseId));
            this.CreateMap<Resource, ResouceDetailsViewModel>();
            this.CreateMap<ResourceDetailsServiceModel, ResouceDetailsViewModel>();
        }
    }
}
