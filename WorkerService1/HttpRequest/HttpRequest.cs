using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WorkerService1;
using WorkerService1.HttpRequest;

namespace WorkerService1.Services
{
    public class HttpRequest : IHttpRequest
    {
        private readonly HttpClient _httpClient;

        public HttpRequest()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7091/") // Kendi API url'ini buraya yaz
            };
        }

        private void AttachToken()
        {
            var token = SessionManager.Token;

            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public async Task<T> GetAllAsync<T>(string url)
        {
            AttachToken();
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> GetByIdAsync<T>(string url, int id)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"{url}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var response = await _httpClient.DeleteAsync($"{url}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync(string url, object data)
        {
            AttachToken();
            var response = await _httpClient.PutAsJsonAsync(url, data);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PostAsync(string url, object data)
        {
            AttachToken();
            var response = await _httpClient.PostAsJsonAsync(url, data);
            return response.IsSuccessStatusCode;
        }

        public async Task<TResponse?> PostAsync<TResponse>(string url, object body)

        {
            AttachToken();
            var json = JsonConvert.SerializeObject(body,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PostAsync(url, content);
            if (!resp.IsSuccessStatusCode)
                return default;

            var str = await resp.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(str))
                return default;

            return JsonConvert.DeserializeObject<TResponse>(str);
        }
    }
}
