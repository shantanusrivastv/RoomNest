using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.DTO
{
    /// <summary>
    /// Response containing available rooms for the requested period
    /// </summary>
    public class AvailabilityResponse
    {
        /// <summary>
        /// Name of the hotel
        /// </summary>
        public string HotelName { get; set; }

        /// <summary>
        /// Check-in date requested
        /// </summary>
        public DateTimeOffset CheckInDate { get; set; }

        /// <summary>
        /// Check-out date requested
        /// </summary>
        public DateTimeOffset CheckOutDate { get; set; }

        /// <summary>
        /// Total accommodation capacity of all available rooms
        /// </summary>
        public int MaximumCapacity => AvailableRooms.Sum(x => x.Capacity);

        /// <summary>
        /// Number of available rooms matching the criteria
        /// </summary>
        public int TotalAvailableRooms => AvailableRooms.Count;

        /// <summary>
        /// Number of guests requested in the search
        /// </summary>
        public int RequestedGuestCount { get; set; }

        /// <summary>
        /// List of available room details
        /// </summary>
        public List<RoomDto> AvailableRooms { get; set; } = new();
    }
}
