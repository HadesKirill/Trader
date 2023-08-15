using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraderBlazor.Models
{
    public class UploadDataModel
    {
        public string PageName { get; set; }
        public string Text { get; set; }
        public List<byte[]> Images { get; set; }
    }
}
