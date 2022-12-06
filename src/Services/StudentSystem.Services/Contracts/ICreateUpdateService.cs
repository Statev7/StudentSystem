namespace StudentSystem.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ICreateUpdateService
    {
        Task CreateEntityAsync<T>(T model);

        Task<bool> UpdateEntityAsync<T>(int id, T model);
    }
}
