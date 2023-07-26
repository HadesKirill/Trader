using Microsoft.AspNetCore.Components;
using MyTraderBlazor.Models;
using System.Diagnostics;

namespace MyTraderBlazor.Controllers
{
    public partial class DataController : ComponentBase
    {
        public ApiService ApiService = new ApiService();

        public List<TextModel> TextData;

        public async Task LoadData()
        {
             TextData = await ApiService.GetTextDataAsync();
        }

        public async Task UploadData(string text, List<byte[]> images)
        {
            var uploadData = new UploadDataModel
            {
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
    }
}
