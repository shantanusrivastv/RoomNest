using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomNest.Entities;

namespace RoomNest.Infrastructure.Entity_Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(e => e.BookingId);
            builder.Property(e => e.BookingReference)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(e => e.GuestName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.GuestEmail)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.TotalAmount)
                .HasPrecision(18, 2);

            builder.HasMany(e => e.BookedRoom)
                .WithOne(r => r.Booking)
                .HasForeignKey(e => e.BookingId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(e => e.BookingReference)
                  .IsUnique();
        }
    }

}
