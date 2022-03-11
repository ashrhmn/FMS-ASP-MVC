using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models
{
    public class TransportModelSR
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> AvailableSeat { get; set; }
        public string FromStopage { get; set; }
        public string ToStopage { get; set; }
        public int FromStopageId { get; set; }
        public int ToStopageId { get; set; }
        public string Day { get; set; }
        public Nullable<int> Time { get; set; }
    }
}