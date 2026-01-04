using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proeflokaal.Models
{
    public class MenuItemModel
    {
        public int Id { get; set; }
        public string MenuItem { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}
