namespace StudentSystem.Data.Seed.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;

    public interface ISeeder
    {
        Task SeedAsync(IServiceScope serviceScope);
    }
}
