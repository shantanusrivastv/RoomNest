using RoomNest.Common;

namespace RoomNest.Model
{
    public class Hotel
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public int TotalRooms => 6; //Todo
        public Dictionary<RoomType, int> AvailableRoomsByType { get; set; }

    }
}
