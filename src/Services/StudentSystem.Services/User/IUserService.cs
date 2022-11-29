namespace StudentSystem.Services.User
{
    using System.Threading.Tasks;

    using StudentSystem.Services.User.Models;

    public interface IUserService 
	{
        Task UpdateAsync(string id, UpdateUserServiceModel user);

        Task<TEntity> GetByIdAsync<TEntity>(string id); 
    }
}
