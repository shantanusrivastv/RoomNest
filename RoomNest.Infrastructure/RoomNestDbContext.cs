using Microsoft.EntityFrameworkCore;
using RoomNest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // Hotel Configuration
            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.HasKey(e => e.HotelId);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(500);
                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.HasIndex(e => e.Name);
                entity.HasMany(e => e.Rooms)
                      .WithOne(r => r.Hotel)
                      .HasForeignKey(r => r.HotelId)
                      .OnDelete(DeleteBehavior.Restrict);  // prevents accidental hotel deletion

            });

            // Room Configuration
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.RoomId);
                entity.Property(e => e.RoomNumber)
                    .IsRequired()
                    .HasMaxLength(5);
                entity.Property(e => e.RoomType)
                    .IsRequired()
                    .HasConversion<string>();
                //entity.Property(e => e.PricePerNight)
                //    .HasPrecision(18, 2);

                //entity.HasOne(e => e.Hotel)
                //    .WithMany(h => h.Rooms)
                //    .HasForeignKey(e => e.HotelId)
                //    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Bookings)
                    .WithOne(b => b.Room)
                    .HasForeignKey(b => b.RoomId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.HotelId, e.RoomNumber })
                    .IsUnique();
            });

            // Booking Configuration
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingId);
                entity.Property(e => e.BookingReference)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.GuestName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.GuestEmail)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.TotalAmount)
                    .HasPrecision(18, 2);

                entity.HasOne(e => e.Room)
                    .WithMany(r => r.Bookings)
                    .HasForeignKey(e => e.RoomId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasIndex(e => e.BookingReference)
                      .IsUnique();
                
                entity.HasIndex(e => new {e.HotelId, e.RoomId, e.CheckInDate, e.CheckOutDate });
            });
        }
    }
}
