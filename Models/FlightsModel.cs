using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models
{
    public class FlightsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> FromStopageId { get; set; }
        public Nullable<int> ToStopageId { get; set; }
        public Nullable<int> MaximumSeat { get; set; }
        public string From { get; set; }
        public string Destination { get; set; }

        public int Fare { get; set; }
    }
}