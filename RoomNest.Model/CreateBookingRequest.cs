using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RoomNest.DTO
{
    /// <summary>
    /// Request payload for creating a new booking
    /// </summary>
    public class CreateBookingRequest
    {
        /// <summary>
        /// Hotel identifier where the booking is being made
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "Hotel ID is required")]
        public int HotelId { get; set; }

        /// <summary>
        /// List of room identifiers to book
        /// </summary>
        /// <example>[ 101, 102 ]</example>
        [Required(ErrorMessage = "At least one room must be specified")]
        [MinLength(1, ErrorMessage = "Must book at least one room")]
        public int[] RoomIds { get; set; }

        /// <summary>
        /// Check-in date for the booking
        /// </summary>
        /// <example>2025-10-25</example>
        [Required(ErrorMessage = "Check-in date is required")]
        public DateTimeOffset CheckInDate { get; set; }

        /// <summary>
        /// Check-out date for the booking
        /// </summary>
        /// <example>2025-10-28</example>
        [Required(ErrorMessage = "Check-out date is required")]
        public DateTimeOffset CheckOutDate { get; set; }

        /// <summary>
        /// Number of guests checking in
        /// </summary>
        /// <example>2</example>
        [Required(ErrorMessage = "Number of guests is required")]
        [Range(1, 10, ErrorMessage = "Number of guests must be between 1 and 10")]
        public int NumberOfGuests { get; set; }

        /// <summary>
        /// Primary guest information
        /// </summary>
        [Required(ErrorMessage = "Guest information is required")]
        public GuestInfo Guest { get; set; }
    }


}
