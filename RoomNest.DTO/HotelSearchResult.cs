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
        public int TotalRooms { get; set; }
        public List<RoomTypeSummary> RoomTypes { get; set; } = new();
    }

    public class RoomTypeSummary
    {
        public RoomType RoomType { get; set; }
        public int Count { get; set; }
        public int Capacity => RoomType == RoomType.Single ? 1 : 2;
    }
}