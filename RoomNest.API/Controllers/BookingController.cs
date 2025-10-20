using Microsoft.AspNetCore.Mvc;
using RoomNest.DTO;
using RoomNest.Services;

namespace RoomNest.API.Controllers
{
    /// <summary>
    /// Provides endpoints to manage hotel room bookings, including creating and retrieving bookings.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// </summary>
        /// <param name="bookingService">The booking service used to manage booking operations.</param>
        public BookingController(IBookingService bookingService) => _bookingService = bookingService;

        /// <summary>
        /// Creates a new booking for a room.
        /// </summary>
        /// <param name="request">The booking details including dates, room, and guest information.</param>
        /// <returns>
        /// Returns a <see cref="BookingResponse"/>  representing the created booking.
        /// </returns>
        /// <response code="201">Booking successfully created.</response>
        /// <response code="400">Invalid request data, e.g. check-in date is after check-out date or number of guests is invalid.</response>
        [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> CreateBooking(CreateBookingRequest request)
        {
            if (request.CheckInDate >= request.CheckOutDate)
                return BadRequest("Check-in date must be before check-out date");

            if (request.NumberOfGuests <= 0)
                return BadRequest("Number of guests must be greater than zero");

            var response = await _bookingService.CreateBookingAsync(request);
            return CreatedAtAction(nameof(GetBooking), new { reference = response.BookingReference }, response);
        }

        /// <summary>
        /// Retrieves the details of an existing booking using its unique reference.
        /// </summary>
        /// <param name="reference">The booking reference identifier.</param>
        /// <returns>
        /// A <see cref="BookingResponse"/> object if found; otherwise, <c>404 Not Found</c>.
        /// </returns>
        /// <response code="200">Booking found and returned successfully.</response>
        /// <response code="404">No booking found with the specified reference.</response>
        [HttpGet("{reference}")]
        [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingResponse>> GetBooking(string reference)
        {
            var booking = await _bookingService.GetBookingByReferenceAsync(reference);
            return booking is null ? NotFound() : Ok(booking);
        }
    }
}