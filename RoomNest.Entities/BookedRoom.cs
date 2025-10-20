namespace RoomNest.Entities
{
    public class BookedRoom
    {
        public int BookingRoomId { get; set; }
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        
        public virtual Booking Booking { get; set; } = null!;
        public virtual Room Room { get; set; } = null!;
    }

}
