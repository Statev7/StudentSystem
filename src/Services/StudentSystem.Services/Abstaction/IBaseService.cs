namespace StudentSystem.Services.Abstaction
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBaseService
    {
        IQueryable<T> GetAllAsQueryable<T>(bool withDeleted = false);

        Task<IEnumerable<T>> GetAllAsync<T>(bool withDeleted = false);

        Task<T> GetByIdAsync<T>(int id);

        Task DeleteAsync(int id);

        IEnumerable<T> Paging<T>(IList<T> data, int currentPage, int entitiesPerPage);

        IQueryable<T> Paging<T>(IQueryable<T> data, int currentPage, int entitiesPerPage);

        Task<bool> IsExistAsync(int id);

        Task<int> GetCountAsync(bool withDeleted = false);
    }
}
