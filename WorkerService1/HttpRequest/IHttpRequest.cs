using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService1.HttpRequest
{
    public interface IHttpRequest
    {
        Task<T> GetAllAsync<T>(string url);
        Task<T> GetByIdAsync<T>(string url, int id);
        Task<bool> PostAsync(string url, object data);
        Task<bool> PutAsync(string url, object data);
        Task<bool> DeleteAsync(string url, int id);
        Task<TResponse?> PostAsync<TResponse>(string url, object body);
    }
}
