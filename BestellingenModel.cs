using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proeflokaal.Models
{
    public class BestellingenModel
    {
        public int BestellingID { get; set; }
        public int ReserveringsnummerID { get; set; }

        public DateTime BestelDatum { get; set; } = DateTime.Now;
        public int TafelNummer { get; set; }

        public string Status { get; set; }
    }
}
