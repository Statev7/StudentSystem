namespace StudentSystem.Services.Contracts
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IPageing
    {
        IEnumerable<T> Paging<T>(IList<T> data, int currentPage, int entitiesPerPage);

        IQueryable<T> Paging<T>(IQueryable<T> data, int currentPage, int entitiesPerPage);
    }
}
