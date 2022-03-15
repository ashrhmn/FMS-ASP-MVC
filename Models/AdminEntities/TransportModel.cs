using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models.AdminEntities
{
    public class TransportModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> FromStopageId { get; set; }
        public Nullable<int> ToStopageId { get; set; }
        public Nullable<int> MaximumSeat { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string From { get; set; }
        public string Destination { get; set; }
        public string CreatorName { get; set; }
        public int? AvailableSeats { get; set; }
        public string FromCountry { get; set; }
        public string DestinationCountry { get; set; }
        public string Day { get; set; }
        public int? Time { get; set; }
        public List<TransportScheduleModel> TransportSchedules { get; set; }
        public TransportModel()
        {
            TransportSchedules = new List<TransportScheduleModel>();
        }
    }
}