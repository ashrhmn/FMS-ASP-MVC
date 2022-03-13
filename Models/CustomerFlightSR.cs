using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models
{
    public class CustomerFlightSR
    {
        public Nullable<int> TransportId { get; set; }

        public int TktId { get; set; }
        public string TName { get; set; }
        public Nullable<int> SeatNo { get; set; }

        public Nullable<int> TFare { get; set; }
        public string SeatClass { get; set; }
        public string ToStopage { get; set; }
        public string FromStopage { get; set; }

        public string Status { get; set; }
        public Nullable<DateTime> StartTime { get; set; }
       
    }
}