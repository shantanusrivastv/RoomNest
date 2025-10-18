using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Model
{
    public class AvailabilityResponse
    {
        //public int HotelId { get; set; }
        public string HotelName { get; set; }
        public Dictionary<RoomType, int> AvailableRoomsByType { get; set; }
        public List<Room> RoomTypes { get; set; } = new();
        public List<Room> AvailableRooms { get; set; }
        public int TotalRooms => 6; //Todo
        public int TotalAvailableRooms => AvailableRooms.Sum(x => x.Capacity);
    }
}
