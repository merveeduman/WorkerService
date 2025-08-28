using System.Threading.Tasks;

namespace WorkerService1.Services
{
    public interface IPokemonService
    {
        Task LoginAsync(string username, string password);
        Task GetAndSavePokemonAsync();
    }
}
