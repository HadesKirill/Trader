using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraderBlazor.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int BlockId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } 
        public DateTime CommentDateTime { get; set; }
        public string CommentText { get; set; }

        public string GetDate()
        {
            return CommentDateTime.ToString("g", new CultureInfo("ru-RU"));
        }
    }
}
