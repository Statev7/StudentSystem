namespace StudentSystem.Services.Abstaction
{
    using System.Threading.Tasks;

    public interface ICreateUpdateService
    {
        Task CreateEntityAsync<T>(T model);

        Task<bool> UpdateEntityAsync<T>(int id, T model);
    }
}
