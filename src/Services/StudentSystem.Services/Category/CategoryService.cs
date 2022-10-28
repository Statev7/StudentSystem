namespace StudentSystem.Services.Category
{
	using System.Linq;
	using System.Threading.Tasks;

	using AutoMapper;
	using Microsoft.EntityFrameworkCore;

	using StudentSystem.Data.Models.StudentSystem;
	using StudentSystem.Services.Abstaction;
    using StudentSystem.Web.Data;

    public class CategoryService : BaseService<Category>, ICategoryService
	{
		public CategoryService(StudentSystemDbContext dbContext, IMapper mapper) 
			: base(dbContext, mapper)
		{
		}

		public async Task<int> GetIdByNameAsync(string name)
		{
			var categoryId = await this.DbSet
				.Where(c => c.Name.ToLower() == name.ToLower())
				.Select(c => c.Id)
				.SingleOrDefaultAsync();

			return categoryId;
		}
	}
}
