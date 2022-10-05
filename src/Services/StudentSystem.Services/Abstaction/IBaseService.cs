namespace StudentSystem.Services.Abstaction
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBaseService
    {
        IQueryable<T> GetAllAsQueryable<T>(bool withDeleted = false);

        IQueryable<T> PageingAsQueryable<T>(int currentPage, int lessonsPerPage);

        Task<T> GetByIdAsync<T>(int id);

        Task CreateAsync<T>(T model);

        Task<bool> UpdateAsync<T>(int id, T model);

        Task<bool> DeleteAsync(int id);

        Task<bool> IsExistAsync(int id);

        Task<int> GetCountAsync(bool withDeleted = false);
    }
}
