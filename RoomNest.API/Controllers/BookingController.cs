using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomNest.Common;
using RoomNest.Model;

namespace RoomNest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Produces("application/json")]
    public class BookingController : ControllerBase
    {
        BookingResponse response = new BookingResponse()
        {
            BookingReference = Guid.NewGuid(),
            CheckInDate = DateTimeOffset.UtcNow,
            CheckOutDate = DateTimeOffset.UtcNow.AddDays(2),
            CreatedAt = DateTimeOffset.UtcNow,
            GuestEmail = "kumarshantanu@hotmail.com",
            GuestName = "Kumar Shantanu",
            NumberOfGuests = 2,
            Rooms = new List<Room>
            {
                new Room
                {
                    HotelId = 1,
                    RoomId = 2,
                    RoomType = RoomType.Deluxe
                },
                new Room
                {
                    HotelId = 1,
                    RoomId = 4,
                    RoomType = RoomType.Double
                }
            }


        };

        //[HttpPost("bookingRequest")] it will add addiotnal url do we need it
        //[ProducesResponseType(typeof(BookingDto), StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<BookingResponse>> CreateBooking(CreateBookingRequest request)
        {
            if (request.CheckInDate >= request.CheckOutDate)
                return BadRequest("Check-in date must be before check-out date");

            if (request.NumberOfGuests <= 0)
                return BadRequest("Number of guests must be greater than zero");

            try
            {
                //var booking = await _bookingService.BookRoomAsync(request);
                return CreatedAtAction(nameof(GetBooking), new { reference = response.BookingReference }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{reference}")]
        [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingResponse>> GetBooking(string reference)
        {
            //var booking = await _bookingService.GetBookingByReferenceAsync(reference);
            //return booking is null ? NotFound() : Ok(booking);
            return Ok(response);
        }
    }
}
