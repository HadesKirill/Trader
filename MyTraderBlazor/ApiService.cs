using MyTraderBlazor.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.Logging;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace MyTraderBlazor
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://traderapi.na4u.ru"; // Замените на адрес вашего API

        private string _token;
        private bool _isAdmin;
        private bool _isVip;
        private int _id;

        public int getId()
        {
            return _id;
        }

        public bool isVip()
        {
            return _isVip;
        }

        public void setToken(string token)
        {
            _token = token;

            if (token != null)
            {
                _isAdmin = ParseAdminStatusFromToken(_token);
                _id = ParseIdFromToken(_token);
            }

            else
            {
                _isAdmin = false;
                _id = -1;
            }
        }

        public bool isAdmin()
        {
            return _isAdmin;
        }

        public ApiService()
        {
            _token = null;
            _isAdmin = false;
            _isVip = false;
            var handler = new HttpClientHandler();
            // Включите поддержку SSL (TLS)
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(ApiBaseUrl);
        }

        public async Task<List<TextModel>> GetTextDataAsync(string pageName)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                Converters = { new ByteArrayConverter() }
            };
            return await _httpClient.GetFromJsonAsync<List<TextModel>>($"/data/{pageName}", jsonOptions);
        }

        public async Task<bool> UploadDataAsync(UploadDataModel uploadData)
        {
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Token is not available. Please log in.");
            }

            // Set the Authorization header with the token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"/upload/{uploadData.PageName}", uploadData);

                _httpClient.DefaultRequestHeaders.Authorization = null;

                return response.IsSuccessStatusCode;
            }

            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Ошибка при загрузке блока: " + ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteBlockAsync(string pageName, int blockId)
        {
            // Check if the token is available
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Token is not available. Please log in.");
            }

            // Set the Authorization header with the token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            try
            {
                // Perform the DELETE request
                var response = await _httpClient.DeleteAsync($"/data/{pageName}/{blockId}");

                // Clear the Authorization header after the request is complete (optional)
                _httpClient.DefaultRequestHeaders.Authorization = null;

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Ошибка при удалении блока текста и связанных с ним картинок: " + ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateTextAsync(string pageName, int blockId, string newText)
        {
            // Check if the token is available
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Token is not available. Please log in.");
            }

            // Set the Authorization header with the token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            try
            {
                var updateData = new
                {
                    PageName = pageName,
                    BlockId = blockId,
                    Text = newText
                };

                // Perform the PUT request with the updated text
                var response = await _httpClient.PutAsJsonAsync($"/update/{pageName}/{blockId}", updateData);

                // Clear the Authorization header after the request is complete (optional)
                _httpClient.DefaultRequestHeaders.Authorization = null;

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Ошибка при обновлении текста: " + ex.Message);
                throw;
            }
        }

        public async Task<bool> RegisterAsync(string username, string email, string password)
        {
            try
            {
                var user = new { username, email, password };
                var response = await _httpClient.PostAsJsonAsync("/register", user);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при регистрации пользователя: " + ex.Message);
                return false;
            }
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            try
            {
                var user = new { username, password };
                var response = await _httpClient.PostAsJsonAsync("/login", user);
                if (response.IsSuccessStatusCode)
                {
                    _token = await response.Content.ReadAsStringAsync();
                    // Проверка, является ли пользователь администратором, на основе содержимого токена
                    _isAdmin = ParseAdminStatusFromToken(_token);
                    _id = ParseIdFromToken(_token);
                    return _token;
                }
                else
                {
                    Debug.WriteLine("Ошибка при входе пользователя: " + response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при входе пользователя: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateUserAsync(int userId, string username, string email, string password = null)
        {
            try
            {
                var user = new { username, email, password };
                var response = await _httpClient.PutAsJsonAsync($"/user/{userId}", user);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при обновлении данных пользователя: " + ex.Message);
                return false;
            }
        }


        private bool ParseAdminStatusFromToken(string token)
        {
            try
            {
              

                // Используем Newtonsoft.Json для десериализации JSON строки
                dynamic tokenObject = JsonConvert.DeserializeObject<dynamic>(token);
                token = tokenObject.token;

                var handler = new JwtSecurityTokenHandler();
                var secretKey = "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC4XzW80yWUGk8+";

                // Указываем параметры для валидации токена
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                // Декодируем и валидируем токен
                var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

                // Получаем значение isAdmin из токена
                var isAdminClaim = principal.Claims.FirstOrDefault(c => c.Type == "admin");

                if (isAdminClaim != null && isAdminClaim.Value == "1")
                {
                    _isAdmin = true;
                    return isAdmin();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при декодировании токена: " + ex.Message);
            }

            return false;
        }

        private int ParseIdFromToken(string token)
        {
            try
            {
                // Используем Newtonsoft.Json для десериализации JSON строки
                dynamic tokenObject = JsonConvert.DeserializeObject<dynamic>(token);
                token = tokenObject.token;

                var handler = new JwtSecurityTokenHandler();
                var secretKey = "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC4XzW80yWUGk8+";

                // Указываем параметры для валидации токена
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                // Декодируем и валидируем токен
                var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

                // Получаем значение isAdmin из токена
                var id = principal.Claims.FirstOrDefault(c => c.Type == "user");

                if (id != null)
                {
                    _id = Convert.ToInt32(id.Value);
                    return getId();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при декодировании токена: " + ex.Message);
            }

            return -1;
        }


        public async Task<List<CommentModel>> GetCommentsAsync(int blockId)
        {
            return await _httpClient.GetFromJsonAsync<List<CommentModel>>($"/comments/{blockId}");
        }

        public async Task<bool> UploadCommentAsync(UploadCommentModel uploadComment)
        {
            // Вам может потребоваться проверка наличия токена, аутентификация и добавление заголовков,
            // если требуется для работы с комментариями.

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"/comment/{uploadComment.BlockId}", uploadComment);

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Ошибка при загрузке комментария: " + ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Токен не доступен. Пожалуйста, выполните вход.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"/comment/{commentId}");

                _httpClient.DefaultRequestHeaders.Authorization = null;

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Ошибка при удалении комментария: " + ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateCommentAsync(int commentId, string newCommentText)
        {
            if (string.IsNullOrEmpty(_token))
            {
                throw new InvalidOperationException("Токен не доступен. Пожалуйста, выполните вход.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            try
            {
                var requestData = new { commentText = newCommentText };
                var response = await _httpClient.PutAsJsonAsync($"/comment/{commentId}", requestData);

                _httpClient.DefaultRequestHeaders.Authorization = null;

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Ошибка при обновлении комментария: " + ex.Message);
                throw;
            }
        }

    }
}
