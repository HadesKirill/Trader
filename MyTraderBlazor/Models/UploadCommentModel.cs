using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraderBlazor.Models
{
    public class UploadCommentModel
    {
        public int BlockId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
    }

}
