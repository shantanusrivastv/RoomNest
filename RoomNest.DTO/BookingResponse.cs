namespace RoomNest.DTO
{
    /// <summary>
    /// Unique booking identifier
    /// </summary>
    public class BookingResponse
    {
        /// <summary>
        /// Booking reference number for customer communication
        /// </summary>
        /// <example>BK-2025-001234</example>
        public string BookingReference { get; set; } = String.Empty;
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public int NumberOfGuests { get; set; }

        /// <summary>
        /// Check-in date
        /// </summary>
        public DateTimeOffset CheckInDate { get; set; }

        /// <summary>
        /// Check-out date
        /// </summary>
        public DateTimeOffset CheckOutDate { get; set; }

        /// <summary>
        /// Booked room identifiers
        /// </summary>
        public List<RoomDto> Rooms { get; set; } = new();

        /// <summary>
        /// Booking creation timestamp
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }
    }
}