using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RoomNest.DTO
{
    /// <summary>
    /// Request payload for checking room availability
    /// </summary>
    public class AvailabilityRequest
    {
        /// <summary>
        /// Hotel identifier to check availability for
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "Hotel ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Hotel ID must be greater than 0")]
        public int HotelId { get; set; }
        /// <summary>
        /// Check-in date and time
        /// </summary>
        /// <example>2025-10-25T14:00:00Z</example>
        [Required(ErrorMessage = "Check-in date is required")]
        public DateTimeOffset CheckInDate { get; set; }

        /// <summary>
        /// Check-out date and time
        /// </summary>
        /// <example>2025-10-28T11:00:00Z</example>
        [Required(ErrorMessage = "Check-out date is required")]
        public DateTimeOffset CheckOutDate { get; set; }

        /// <summary>
        /// Number of guests requiring accommodation
        /// </summary>
        /// <example>2</example>
        [Required(ErrorMessage = "Guest count is required")]
        [Range(1, 10, ErrorMessage = "Guest count must be between 1 and 10")]
        public int GuestCount { get; set; }
    }
}
