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
    
    public partial class TransportSchedule
    {
        public int Id { get; set; }
        public Nullable<int> TransportId { get; set; }
        public Nullable<int> FromStopageId { get; set; }
        public Nullable<int> ToStopageId { get; set; }
        public string Day { get; set; }
        public Nullable<int> Time { get; set; }
    
        public virtual Stopage Stopage { get; set; }
        public virtual Stopage Stopage1 { get; set; }
        public virtual Transport Transport { get; set; }
    }
}