using Microsoft.AspNetCore.Components;
using MyTraderBlazor.Models;
using System.Diagnostics;

namespace MyTraderBlazor.Controllers
{
    public partial class DataController : ComponentBase
    {
        public ApiService ApiService = new ApiService();

        public List<TextModel> TextData;
        public string CurrentPageName = ""; // Здесь укажите имя текущей страницы

        public event EventHandler<string> TokenReceived;

        public bool isAuthorized = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private List<TextModel> SortData(List<TextModel> list)
        {
            return list.OrderByDescending(x => x.LoadDate).ToList();
        }

        public async Task LoadData()
        {
            TextData = SortData(await ApiService.GetTextDataAsync(CurrentPageName));
        }

        public async Task UploadData(string text, List<byte[]> images)
        {
            var uploadData = new UploadDataModel
            {
                PageName = CurrentPageName,
                Text = text,
                Images = images
            };

            var success = await ApiService.UploadDataAsync(uploadData);

            if (success)
            {
                await LoadData(); // Обновление данных после успешной загрузки
            }
            else
            {
                // Обработка ошибки при загрузке
            }
        }

        public async Task DeleteBlock(int blockId)
        {
            var success = await ApiService.DeleteBlockAsync(CurrentPageName, blockId);
            if (success)
            {
                await LoadData(); // Обновляем данные после успешного удаления блока
            }
            else
            {
                // Обработка ошибки при удалении
            }
        }

        public async Task UpdateTextBlock(int blockId, string text)
        {
            var success = await ApiService.UpdateTextAsync(CurrentPageName, blockId, text);
            if (success)
            {
                await LoadData(); // Обновляем данные после успешного удаления блока
            }
            else
            {
                // Обработка ошибки при удалении
            }
        }


        public async Task RegisterUser(string username, string email, string password)
        {
            var success = await ApiService.RegisterAsync(username, email, password);
            if (success)
            {
                // Обработка успешной регистрации
            }
            else
            {
                // Обработка ошибки при регистрации
            }
        }

        public async Task LoginUser(string username, string password)
        {
            var token = await ApiService.LoginAsync(username, password);
            if (!string.IsNullOrEmpty(token))
            {
                TokenReceived?.Invoke(this, token);
                isAuthorized = true;
            }
            else
            {
                // Обработка ошибки при входе
            }
        }

        public async Task UpdateUserData(int userId, string username, string email, string password = null)
        {
            var success = await ApiService.UpdateUserAsync(userId, username, email, password);
            if (success)
            {
                // Обработка успешного обновления данных пользователя
            }
            else
            {
                // Обработка ошибки при обновлении данных пользователя
            }
        }

    }
}
