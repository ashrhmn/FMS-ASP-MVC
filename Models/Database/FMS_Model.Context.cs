﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Flight_ManagementEntities : DbContext
    {
        public Flight_ManagementEntities()
            : base("name=Flight_ManagementEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AgeClassEnum> AgeClassEnums { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<PurchasedTicket> PurchasedTickets { get; set; }
        public DbSet<SeatClassEnum> SeatClassEnums { get; set; }
        public DbSet<SeatInfo> SeatInfos { get; set; }
        public DbSet<Stopage> Stopages { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<UserRoleEnum> UserRoleEnums { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TransportSchedule> TransportSchedules { get; set; }
    }
}
