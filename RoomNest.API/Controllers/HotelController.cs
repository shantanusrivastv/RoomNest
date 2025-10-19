using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomNest.Entities;
using RoomNest.Model;
using RoomNest.Services;
using System.Threading;

namespace RoomNest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet("{hotelName}")]
        public async Task<IActionResult> GetHotel(string hotelName)
        {
            var hotel = await _hotelService.FindHotelByNameAsync(hotelName);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        [HttpGet]
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
