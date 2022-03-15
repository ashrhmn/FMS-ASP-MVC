using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models.AdminEntities
{
    public class TransportScheduleModel
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public int? Time { get; set; }
        [DisplayName("From Airport")]
        public int? FromStopageId { get; set; }
        [DisplayName("To Airport")]
        public int? ToStopageId { get; set; }
        public string FromAirport { get; set; }
        public string ToAirport { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string FromCountry { get; set; }
        public string ToCountry { get; set; }
        public int? MaximumSeat { get; set; }

        public string FromStopageName { get; set; }
        public string FromStopageCityName { get; set; }
        public string FromStopageCountryName { get; set; }

        public string ToStopageName { get; set; }
        public string ToStopageCityName { get; set; }
        public string ToStopageCountryName { get; set; }

    }
}