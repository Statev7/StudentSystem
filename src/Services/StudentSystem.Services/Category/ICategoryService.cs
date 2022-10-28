namespace StudentSystem.Services.Category
{
    using System.Threading.Tasks;

    using StudentSystem.Services.Abstaction;

    public interface ICategoryService : IBaseService
	{
        Task<int> GetIdByNameAsync(string name);
	}
}
