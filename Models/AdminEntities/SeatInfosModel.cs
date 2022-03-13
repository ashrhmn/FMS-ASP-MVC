using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models.AdminEntities
{
    public class SeatInfosModel
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<int> SeatNo { get; set; }
        public Nullable<int> TicketId { get; set; }
        public Nullable<int> TransportId { get; set; }
        public Nullable<int> AgeClass { get; set; }
        public Nullable<int> SeatClass { get; set; }
        public string Status { get; set; }
        public int PurchasedById { get; set; }
        public string PurchasedByName { get; set; }
        public string SeatClassName { get; set; }
        public string AircraftName { get; set; }
    }
}