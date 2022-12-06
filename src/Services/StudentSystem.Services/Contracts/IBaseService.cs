namespace StudentSystem.Services.Abstaction
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using StudentSystem.Services.Contracts;

    public interface IBaseService : IPageing
    {
        IQueryable<T> GetAllAsQueryable<T>(bool withDeleted = false);

        Task<IEnumerable<T>> GetAllAsync<T>(bool withDeleted = false);

        Task<T> GetByIdAsync<T>(int id);

        Task DeleteAsync(int id);

        Task<bool> IsExistAsync(int id);

        Task<int> GetCountAsync(bool withDeleted = false);
    }
}
