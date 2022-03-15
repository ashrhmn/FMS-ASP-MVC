using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models
{
    public class TransportModelSR
    {

        public int Id { get; set; }
        public Nullable<int> TransportId { get; set; }

        public string Cname { get; set; }

        public string Cphone { get; set; }
        public string Name { get; set; }
        public Nullable<int> AvailableSeats { get; set; }
        public string FromStopage { get; set; }
        public string ToStopage { get; set; }

        public string SFS { get; set; }
        public string STS { get; set; }

        public string Class { get; set; }
        public int FromStopageId { get; set; }
        public Nullable<int> ClassId { get; set; }

        public int ToStopageId { get; set; }
        public string Day { get; set; }
        public Nullable<int> SFare { get; set; }

        public DateTime Date { get; set; }

        public DateTime MDate { get; set; }

        public string DateSt { get; set; }
        public Nullable<int> Time { get; set; }
    }
}