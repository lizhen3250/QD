﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QD_Tour_Web.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QDTourWebEntities : DbContext
    {
        public QDTourWebEntities()
            : base("name=QDTourWebEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Guide> Guides { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Guide_Package> Guide_Package { get; set; }
        public virtual DbSet<Guide_Reservation> Guide_Reservation { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Vehicle_Package> Vehicle_Package { get; set; }
        public virtual DbSet<Vehicle_Reservation> Vehicle_Reservation { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<Hotel_Package> Hotel_Package { get; set; }
        public virtual DbSet<Hotel_Price> Hotel_Price { get; set; }
        public virtual DbSet<Hotel_Reservation> Hotel_Reservation { get; set; }
        public virtual DbSet<Tour> Tours { get; set; }
        public virtual DbSet<Tour_Package> Tour_Package { get; set; }
        public virtual DbSet<Tour_Price> Tour_Price { get; set; }
        public virtual DbSet<Tour_Reservation> Tour_Reservation { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Hotel_Package_Image> Hotel_Package_Image { get; set; }
    }
}
