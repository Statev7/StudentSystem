namespace StudentSystem.Services.Category
{
    using AutoMapper;

	using StudentSystem.Data.Models.StudentSystem;
	using StudentSystem.Services.Abstaction;
    using StudentSystem.Web.Data;

    public class CategoryService : BaseService<Category>, ICategoryService
	{
		public CategoryService(StudentSystemDbContext dbContext, IMapper mapper) 
			: base(dbContext, mapper)
		{
		}
	}
}
