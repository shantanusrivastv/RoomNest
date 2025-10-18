using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomNest.Model;

namespace RoomNest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private static readonly List<Room> Rooms = new List<Room>
        {
            new Room { RoomId = 1, Type = RoomType.Single},
            new Room { RoomId = 2, Type = RoomType.Double},
            new Room { RoomId = 3, Type = RoomType.Deluxe}
        };

        private static readonly List<Booking> Bookings = new List<Booking>();


        [HttpPost("availability")]
        public async Task<ActionResult<AvailabilityResponse>> CheckAvailability(AvailabilityRequest request)
        {
            try
            {
                //var availableRooms = Rooms
                //.Where(r => r.HotelId == request.HotelId && r.Capacity >= request.NumberOfPeople)
                //.Where(r => !Bookings.Any(b => b.RoomId == r.RoomId))
                //    ((checkIn >= b.CheckIn && checkIn < b.CheckOut) ||
                //     (checkOut > b.CheckIn && checkOut <= b.CheckOut) ||
                //     (checkIn <= b.CheckIn && checkOut >= b.CheckOut))))
                //    .ToList();


                //Need Unit Testing
                var availableRooms = Rooms/*.Where(r => r.HotelId == request.HotelId)*/
                                           .Where(r => !Bookings.Any(b =>
                                                b.RoomId == r.RoomId &&
                                                b.CheckOutDate > request.CheckInDate &&
                                                b.CheckInDate < request.CheckOutDate
                                                ))
                                           .ToList();

                //request.HotelId = hotelId;
                //var result = await _bookingService.CheckAvailabilityAsync(request);
                var result = new AvailabilityResponse()
                {                    
                    HotelName = "Grant Hotel",
                    CheckInDate = request.CheckInDate,
                    CheckOutDate = request.CheckOutDate,
                    NumberOfGuests = request.NumberOfPeople,
                    AvailableRooms = availableRooms,
                    //AvailableRoomsType = new Dictionary<RoomType, int>()
                    //{
                    //    {RoomType.Single, 2 },
                    //    {RoomType.Double, 3 },
                    //    {RoomType.Deluxe, 1 },

                    //}
                };

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
