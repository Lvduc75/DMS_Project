using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DMS_FE.Helpers
{
    public class ApiHelper
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;

        public ApiHelper(IConfiguration configuration)
        {
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5000";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(_apiBaseUrl);
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            return await _httpClient.GetAsync(endpoint);
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
        {
            return await _httpClient.PostAsync(endpoint, content);
        }

        public async Task<HttpResponseMessage> PutAsync(string endpoint, HttpContent content)
        {
            return await _httpClient.PutAsync(endpoint, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            return await _httpClient.DeleteAsync(endpoint);
        }
    }
} 