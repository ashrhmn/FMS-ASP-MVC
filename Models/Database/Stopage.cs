//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Flight_Management_System.Models.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stopage
    {
        public Stopage()
        {
            this.TransportSchedules = new HashSet<TransportSchedule>();
            this.TransportSchedules1 = new HashSet<TransportSchedule>();
            this.PurchasedTickets = new HashSet<PurchasedTicket>();
            this.PurchasedTickets1 = new HashSet<PurchasedTicket>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> RouteIndex { get; set; }
        public Nullable<int> FareFromRoot { get; set; }
    
        public virtual City City { get; set; }
        public virtual ICollection<TransportSchedule> TransportSchedules { get; set; }
        public virtual ICollection<TransportSchedule> TransportSchedules1 { get; set; }
        public virtual ICollection<PurchasedTicket> PurchasedTickets { get; set; }
        public virtual ICollection<PurchasedTicket> PurchasedTickets1 { get; set; }
    }
}
