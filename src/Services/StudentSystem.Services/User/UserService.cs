namespace StudentSystem.Services.User
{
	using System.Linq;
	using System.Threading.Tasks;

	using AutoMapper;
	using AutoMapper.QueryableExtensions;

	using Microsoft.EntityFrameworkCore;

	using StudentSystem.Services.User.Models;
	using StudentSystem.Web.Data;

    public class UserService : IUserService
	{
		private readonly StudentSystemDbContext dbContext;
		private readonly IMapper mapper;

		public UserService(StudentSystemDbContext dbContext, IMapper mapper)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
		}

		public async Task<TEntity> GetByIdAsync<TEntity>(string id)
			=> await this.dbContext
				.Users
				.Where(u => u.Id == id)
				.ProjectTo<TEntity>(this.mapper.ConfigurationProvider)
				.FirstOrDefaultAsync();


        public async Task UpdateAsync(string id, UpdateUserServiceModel user)
		{
			var userFromDb = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

			this.mapper.Map(user, userFromDb);

			this.dbContext.Update(userFromDb);
		 	await this.dbContext.SaveChangesAsync();
		}
	}
}
