using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Model
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public DateTimeOffset CheckInDate { get; set; }
        public DateTimeOffset CheckOutDate { get; set; }
        public Guest Guest { get; set; }
        public decimal TotalAmount { get; set; } //todo
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
