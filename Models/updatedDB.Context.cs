﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeyadLeyaad1.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Database1Entities : DbContext
    {
        public Database1Entities()
            : base("name=Database1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Contribution> Contribution { get; set; }
        public DbSet<Donor> Donor { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<Picture> Picture { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Statuses> Statuses { get; set; }
        public DbSet<SubCategories> SubCategories { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
