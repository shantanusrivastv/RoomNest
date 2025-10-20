using Microsoft.AspNetCore.Mvc;
using RoomNest.API.Middleware;
using RoomNest.DTO;
using RoomNest.Services;

namespace RoomNest.API.Controllers
{
    /// <summary>
    /// Provides endpoints for checking room availability and managing room-related operations.
    /// </summary>
    /// <remarks>
    /// This controller currently supports checking room availability based on user-provided search criteria.
    /// </remarks>
    /// <remarks>
    /// Initialises a new instance of the <see cref="RoomController"/> class.
    /// </remarks>
    /// <param name="roomService">The service responsible for room operations and availability checks.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RoomController(IRoomService roomService) : ControllerBase
    {
        private readonly IRoomService _roomService = roomService;

        /// <summary>
        /// Checks room availability based on the specified search criteria.
        /// </summary>
        /// <param name="request">
        /// The request payload containing hotel ID, check-in and check-out dates, and number of guests.
        /// </param>
        /// <returns>
        /// A <see cref="AvailabilityResponse"/> containing available rooms and related details.
        /// </returns>
        /// <response code="200">Availability information returned successfully.</response>
        /// <response code="400">Invalid request data provided.</response>
        [HttpPost("availability")]
        [ProducesResponseType(typeof(AvailabilityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CheckAvailability(AvailabilityRequest request)
        {
            var result = await _roomService.CheckAvailabilityAsync(request);
            return Ok(result);
        }
    }
}