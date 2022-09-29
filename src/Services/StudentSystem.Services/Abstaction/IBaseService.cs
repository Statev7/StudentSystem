namespace StudentSystem.Services.Abstaction
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBaseService
    {
        IQueryable<T> GetAllAsQueryable<T>(bool withDeleted = false);

        Task<T> GetByIdAsync<T>(int id);

        Task CreateAsync<T>(T model);

        Task<bool> UpdateAsync<T>(int id, T model);

        Task<bool> DeleteAsync(int id);
    }
}
