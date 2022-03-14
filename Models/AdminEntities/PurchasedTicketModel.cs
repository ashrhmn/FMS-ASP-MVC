using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models.AdminEntities
{
    public class PurchasedTicketModel
    {
        public int Id { get; set; }
        public int FromStopageId { get; set; }
        public int ToStopageId { get; set; }
        public int PurchasedBy { get; set; }

        public string PurchasedByName { get; set; }

        public string FromStopageName { get; set; }
        public string FromStopageCityName { get; set; }
        public string FromStopageCountryName { get; set; }

        public string ToStopageName { get; set; }
        public string ToStopageCityName { get; set; }
        public string ToStopageCountryName { get; set; }


        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<int> SeatNo { get; set; }
        public Nullable<int> TransportId { get; set; }
        public string AgeClass { get; set; }
        public string SeatClass { get; set; }
        public Nullable<int> TransportCreatedBy { get; set; }
        public string TransportName { get; set; }
        public string TransportCreatorName { get; set; }
        
        public int Cost { get; set; }




    }
}