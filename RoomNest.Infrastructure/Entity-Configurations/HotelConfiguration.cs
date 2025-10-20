using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomNest.Entities;

namespace RoomNest.Infrastructure.Entity_Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(e => e.HotelId);
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(e => e.Name);
            builder.HasMany(e => e.Rooms)
                  .WithOne(r => r.Hotel)
                  .HasForeignKey(r => r.HotelId)
                  .OnDelete(DeleteBehavior.Restrict);  // prevents accidental hotel deletion
        }
    }
}