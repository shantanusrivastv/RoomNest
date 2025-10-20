using Microsoft.EntityFrameworkCore;
using RoomNest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Infrastructure
{
    public class RoomNestDbContext : DbContext
    {
        public RoomNestDbContext(DbContextOptions<RoomNestDbContext> options) : base(options)
        {
            
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookedRoom> BookedRoom { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //It searches for all the configuration of all entities
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
