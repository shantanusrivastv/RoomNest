using RoomNest.Common;
using System.Text.Json.Serialization;

namespace RoomNest.DTO
{
    /// <summary>
    /// Room details in availability response
    /// </summary>
    public class RoomDto
    {
        /// <summary>
        /// Room identifier
        /// </summary>
        public int RoomId { get; set; }

        [JsonIgnore]
        public int HotelId { get; set; }

        /// <summary>
        /// Type of room (Single, Double, Deluxe)
        /// </summary>
        public RoomType RoomType { get; set; }

        /// <summary>
        /// Guest capacity of the room
        /// </summary>
        public int Capacity => RoomType == RoomType.Single ? 1 : 2;
    }
}