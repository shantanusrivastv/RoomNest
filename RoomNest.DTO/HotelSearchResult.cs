using RoomNest.Common;

namespace RoomNest.DTO
{
    public class HotelSearchResult
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public int TotalRooms => 6; //TODO

        // public int AvailableRooms { get; set; }
        public List<RoomTypeSummary> RoomTypes { get; set; } = new();

        //public int MaxCapacity => RoomTypes.Any() ? RoomTypes.Max(rt => rt.Capacity) : 0;
        //public bool HasAvailableRooms => AvailableRooms > 0;
    }

    public class RoomTypeSummary
    {
        public RoomType RoomType { get; set; }
        public int Count { get; set; }
        public int Capacity => RoomType == RoomType.Single ? 1 : 2;
    }

    //public class HotelRoomSummary
    //{
    //    public RoomType RoomType { get; set; }
    //    public int TotalCount { get; set; }
    //    public int AvailableCount { get; set; }
    //    public int Capacity => RoomType == RoomType.Single? 1 : 2;
    //}
}