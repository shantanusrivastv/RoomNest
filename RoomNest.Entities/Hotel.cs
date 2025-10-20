namespace RoomNest.Entities
{
    public class Hotel
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }

        public virtual List<Room> Rooms { get; set; } = new();
    }
}