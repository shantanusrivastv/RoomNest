using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomNest.Model;

namespace RoomNest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        // In-memory hotel list for demonstration
        private static readonly List<Hotel> Hotels = new List<Hotel>
        {
            new Hotel { Id = 1, Name = "Hotel Sunshine" },
            new Hotel { Id = 2, Name = "Mountain View Inn" }
        };

        [HttpGet("{name}")]
        public ActionResult<Hotel> GetHotelByName(string name)
        {
            var hotel = Hotels.Find(h => h.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }
    }
}
