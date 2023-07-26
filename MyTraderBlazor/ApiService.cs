using MyTraderBlazor.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace MyTraderBlazor
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://192.168.1.64:3000"; // Замените на адрес вашего API

        public ApiService()
        {
            var handler = new HttpClientHandler();
            // Включите поддержку SSL (TLS)
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(ApiBaseUrl);
        }

        public async Task<List<TextModel>> GetTextDataAsync()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                Converters = { new ByteArrayConverter() }
            };
            return await _httpClient.GetFromJsonAsync<List<TextModel>>("/data", jsonOptions);
        }

        public async Task<bool> UploadDataAsync(UploadDataModel uploadData)
        {
            var response = await _httpClient.PostAsJsonAsync("/upload", uploadData);
            return response.IsSuccessStatusCode;
        }
    }
}
