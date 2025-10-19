using Microsoft.AspNetCore.Mvc;
using RoomNest.Services;

namespace RoomNest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DataController : ControllerBase
    {
        private readonly DBSeedService _dbSeedService;

        public DataController()
        {
            _dbSeedService = new DBSeedService();
        }

        /// <summary>
        /// Seed the database with initial test data
        /// </summary>
        /// <returns>Success message</returns>
        [HttpPost("seed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SeedDatabase()
        {
            await _dbSeedService.SeedAsync();
            return Ok(new { message = "Database seeded successfully." });
        }

        /// <summary>
        /// Reset the database by removing all data
        /// </summary>
        /// <returns>Success message</returns>
        [HttpPost("reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetDatabase()
        {
           await _dbSeedService.ResetAsync();
            return Ok(new { message = "Database reset successfully. Use /api/data/seed to populate with initial data." });
        }
    }
}
