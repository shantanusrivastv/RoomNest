using Microsoft.AspNetCore.Mvc;
using RoomNest.DTO;
using RoomNest.Services.Interfaces;

namespace RoomNest.API.Controllers
{
    /// <summary>
    /// Provides endpoints for retrieving hotel information by name or ID.
    /// </summary>
    /// <remarks>
    /// This controller exposes operations to fetch hotel details from the system.
    /// It supports searching either by the hotel's name or its unique identifier.
    /// </remarks>
    /// <remarks>
    /// Initialises a new instance of the <see cref="HotelController"/> class.
    /// </remarks>
    /// <param name="hotelService">The service used for retrieving hotel information.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class HotelController(IHotelService hotelService) : ControllerBase
    {
        private readonly IHotelService _hotelService = hotelService;

        /// <summary>
        /// Retrieves hotel details by hotel name.
        /// </summary>
        /// <param name="hotelName">The name of the hotel to search for.</param>
        /// <returns>
        /// Returns the hotel details if found; otherwise, a <c>404 Not Found</c> response.
        /// </returns>
        /// <response code="200">Hotel found and returned successfully.</response>
        /// <response code="404">No hotel found with the specified name.</response>
        [HttpGet("{hotelName}")]
        [ProducesResponseType(typeof(List<HotelSearchResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHotel(string hotelName)
        {
            var hotel = await _hotelService.FindHotelByNameAsync(hotelName);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        /// <summary>
        /// Retrieves hotel details by unique hotel ID.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel.</param>
        /// <returns>
        /// Returns the hotel details if found; otherwise, a <c>404 Not Found</c> response.
        /// </returns>
        /// <response code="200">Hotel found and returned successfully.</response>
        /// <response code="404">No hotel found with the specified ID.</response>
        [HttpGet]
        [ProducesResponseType(typeof(HotelSearchResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetHotel([FromQuery] int id)
        {
            var hotel = await _hotelService.FindHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }
    }
}