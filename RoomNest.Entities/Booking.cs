using RoomNest.Common;

namespace RoomNest.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; } = Guid.NewGuid().ToString();
        public int HotelId { get; set; }  //Denormalized for faster queries
        public DateTimeOffset CheckInDate { get; set; }
        public DateTimeOffset CheckOutDate { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedAt { get; set; }
        public virtual List<BookedRoom> BookedRoom { get; set; } = new();  // Navigation property
    }
}