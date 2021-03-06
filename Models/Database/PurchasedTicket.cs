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
    
    public partial class PurchasedTicket
    {
        public PurchasedTicket()
        {
            this.SeatInfos = new HashSet<SeatInfo>();
        }
    
        public int Id { get; set; }
        public int FromStopageId { get; set; }
        public int ToStopageId { get; set; }
        public int PurchasedBy { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<SeatInfo> SeatInfos { get; set; }
        public virtual Stopage Stopage { get; set; }
        public virtual Stopage Stopage1 { get; set; }
    }
}
