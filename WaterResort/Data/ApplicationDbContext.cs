using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WaterResort.Models;

namespace WaterResort.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // define model classes
        public DbSet<RestaurantTable> RestaurantTables { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<CurrentReservation> CurrentReservations { get; set; }
        public DbSet<AccountsReceivable> AccountsReceivables { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AccountsReceivable>()
                .Property(m => m.Balance)
                .HasColumnType("decimal(8,2)");
            builder.Entity<Room>()
                .Property(m => m.CostPerNight)
                .HasColumnType("decimal(8,2)");
            builder.Entity<CurrentReservation>()
                .Property(m => m.TotalCost)
                .HasColumnType("decimal(8,2)");
            base.OnModelCreating(builder);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        
    }
}
