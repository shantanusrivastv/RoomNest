using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Model
{
    public class BookingResponse
    {
        public Guid BookingReference { get; set; } = Guid.Empty;
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public int NumberOfGuests { get; set; }
        public DateTimeOffset CheckInDate { get; set; }
        public DateTimeOffset CheckOutDate { get; set; }
        public IList<Room> Rooms { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
