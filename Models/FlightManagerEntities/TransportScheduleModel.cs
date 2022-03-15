using System.ComponentModel;

namespace Flight_Management_System.Models.FlightManagerEntities
{
    public class TransportScheduleModel
    {
        public int Id { get; set; }
        public int TransportId { get; set; }
        public string Day { get; set; }
        public int Time { get; set; }
        public int TimeH { get; set; }
        public int TimeM { get; set; }
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
    }
}