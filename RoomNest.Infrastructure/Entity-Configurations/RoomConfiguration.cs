using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomNest.Entities;

namespace RoomNest.Infrastructure.Entity_Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(e => e.RoomId);
            builder.Property(e => e.RoomNumber)
                .IsRequired()
                .HasMaxLength(5);
            builder.Property(e => e.RoomType)
                .IsRequired()
                .HasConversion<string>();
            //builder.Property(e => e.PricePerNight)
            //    .HasPrecision(18, 2);

            //builder.HasOne(e => e.Hotel)
            //    .WithMany(h => h.Rooms)
            //    .HasForeignKey(e => e.HotelId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.BookedRoom)
                .WithOne(b => b.Room)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.HotelId, e.RoomNumber })
                .IsUnique();
        }
    }
}