using Microsoft.AspNetCore.Components;
using MyTraderBlazor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraderBlazor.Controllers
{
    public partial class CommentController : ComponentBase
    {
        public ApiService ApiService = new ApiService();

        public List<CommentModel> CommentData;
        public int CurrentBlockId; // Здесь укажите текущий айди блока текста

        protected override async Task OnInitializedAsync()
        {
            await LoadComments();
        }

        private List<CommentModel> SortComments(List<CommentModel> list)
        {
            return list.OrderByDescending(x => x.CommentDateTime).ToList();
        }

        public async Task LoadComments()
        {
            CommentData = SortComments(await ApiService.GetCommentsAsync(CurrentBlockId));
        }

        public async Task UploadComment(string commentText, int userId)
        {
            var uploadComment = new UploadCommentModel
            {
                BlockId = CurrentBlockId,
                UserId = userId, // Здесь укажите айди текущего пользователя или получите его из сессии/токена
                CommentText = commentText
            };

            var success = await ApiService.UploadCommentAsync(uploadComment);

            if (success)
            {
                await LoadComments(); // Обновление комментариев после успешной загрузки
            }
            else
            {
                // Обработка ошибки при загрузке комментария
            }
        }

        public async Task DeleteComment(int commentId)
        {
            var success = await ApiService.DeleteCommentAsync(commentId);

            if (success)
            {
                await LoadComments(); // Обновление комментариев после успешного удаления
            }
            else
            {
                // Обработка ошибки при удалении комментария
            }
        }

        public async Task UpdateComment(int commentId, string newCommentText)
        {
            var success = await ApiService.UpdateCommentAsync(commentId, newCommentText);

            if (success)
            {
                await LoadComments(); // Обновление комментариев после успешного обновления
            }
            else
            {
                // Обработка ошибки при обновлении комментария
            }
        }

    }
}
