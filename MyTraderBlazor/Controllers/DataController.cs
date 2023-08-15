using Microsoft.AspNetCore.Components;
using MyTraderBlazor.Models;
using System.Diagnostics;

namespace MyTraderBlazor.Controllers
{
    public partial class DataController : ComponentBase
    {
        public ApiService ApiService = new ApiService();

        public List<TextModel> TextData;
        public string CurrentPageName = ""; 

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
                await LoadData(); 
            }
            else
            {
                
            }
        }

        public async Task DeleteBlock(int blockId)
        {
            var success = await ApiService.DeleteBlockAsync(CurrentPageName, blockId);
            if (success)
            {
                await LoadData(); 
            }
            else
            {
               
            }
        }

        public async Task UpdateTextBlock(int blockId, string text)
        {
            var success = await ApiService.UpdateTextAsync(CurrentPageName, blockId, text);
            if (success)
            {
                await LoadData(); 
            }
            else
            {
                
            }
        }


        public async Task RegisterUser(string username, string email, string password)
        {
            var success = await ApiService.RegisterAsync(username, email, password);
            if (success)
            {
                
            }
            else
            {
                
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
                
            }
        }

        public async Task UpdateUserData(int userId, string username, string email, string password = null)
        {
            var success = await ApiService.UpdateUserAsync(userId, username, email, password);
            if (success)
            {
                
            }
            else
            {
               
            }
        }

    }
}
