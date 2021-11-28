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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<WaterResort.Models.CurrentReservation> CurrentReservations { get; set; }
    }
}
