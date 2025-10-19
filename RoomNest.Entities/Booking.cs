using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; } = Guid.NewGuid().ToString();
        public int RoomId { get; set; }
        public int HotelId { get; set; }  //Todo Denormalized for faster queries
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }

        public int NumberOfGuests { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public virtual Room Room { get; set; } // Navigation property
    }

    public enum BookingStatus
    {
        Confirmed,
        Cancelled,
        CheckedIn,
        CheckedOut
    }
}
