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
        private static readonly List<HotelSearchResult> Hotels = new List<HotelSearchResult>
        {
           new HotelSearchResult()
           {
             HotelId = 1,
            Name = "Grand Plaza Hotel",
            City = "New York",
            Address = "123 Main Street",
            ContactEmail = "info@grandplaza.com",
            ContactPhone = "+1-555-0101",
            //AvailableRooms = 1,
            RoomTypes = new List<RoomTypeSummary>()
            {
                new RoomTypeSummary() { RoomType = RoomType.Deluxe}
            }
           }
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
