using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomNest.Entities;

namespace RoomNest.Infrastructure.Entity_Configurations
{
    public class BookedRoomConfiguration : IEntityTypeConfiguration<BookedRoom>
    {
        public void Configure(EntityTypeBuilder<BookedRoom> builder)
        {
            builder.HasKey(e => e.BookingRoomId);
            builder.HasIndex(e => new { e.BookingId, e.RoomId }).IsUnique();
        }
    }

}
