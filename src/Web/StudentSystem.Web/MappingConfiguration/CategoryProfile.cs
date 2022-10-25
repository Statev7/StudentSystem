namespace StudentSystem.Web.MappingConfiguration
{
    using AutoMapper;

	using StudentSystem.Data.Models.StudentSystem;
	using StudentSystem.ViewModels.Category;

	public class CategoryProfile : Profile
	{
		public CategoryProfile()
		{
			this.CreateMap<Category, CategoryIdNameViewModel>();
		}
	}
}
