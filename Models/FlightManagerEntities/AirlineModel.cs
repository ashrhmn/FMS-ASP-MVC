using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models.FlightManagerEntities
{
    public class AirlineModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? FromStopageId { get; set; }
        public int? ToStopageId { get; set; }
        public string FromAirport { get; set; }
        public string ToAirport { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string FromCountry { get; set; }
        public string ToCountry { get; set; }
        public int SeatCapacity { get; set; }
    }
}