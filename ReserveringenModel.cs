using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proeflokaal.Models
{
    public class ReserveringenModel
    {
        public int ReserveringsnummerID { get; set; }
        public int AantalPersonen { get; set; }
        public string Status { get; set; }
        public int KlantID { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan Tijdslot { get; set; }
    }
}
