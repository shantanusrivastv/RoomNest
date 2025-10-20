using Microsoft.AspNetCore.Mvc;
using RoomNest.Services.Interfaces;

namespace RoomNest.API.Controllers
{
    /// <summary>
    /// Provides administrative endpoints for managing and initialising database data.
    /// </summary>
    /// <remarks>
    /// This controller allows seeding the database with initial test data and resetting it to a clean state.
    /// Typically used in development or testing environments.
    /// </remarks>
    /// <remarks>
    /// Initialises a new instance of the <see cref="DataController"/> class.
    /// </remarks>
    /// <param name="dbSeedService">The service responsible for seeding and resetting the database.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DataController(IDBSeedService dbSeedService) : ControllerBase
    {
        private readonly IDBSeedService _dbSeedService = dbSeedService;

        /// <summary>
        /// Seeds the database with predefined test data.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to populate the database with sample data for testing or demo purposes.
        /// If data already exists, seeding may overwrite or append depending on implementation.
        /// </remarks>
        /// <returns>
        /// Returns a success message indicating that the seeding operation completed successfully.
        /// </returns>
        /// <response code="200">Database seeded successfully.</response>
        [HttpPost("seed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SeedDatabase()
        {
            await _dbSeedService.SeedAsync();
            return Ok(new { message = "Database seeded successfully." });
        }

        /// <summary>
        /// Resets the database by removing all existing records.
        /// </summary>
        /// <remarks>
        /// This operation permanently deletes all data from the database.
        /// Use the <c>/api/data/seed</c> endpoint afterwards to reinitialise test data.
        /// </remarks>
        /// <returns>
        /// Returns a success message once the reset operation completes.
        /// </returns>
        /// <response code="200">Database reset successfully.</response>
        [HttpPost("reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetDatabase()
        {
            await _dbSeedService.ResetAsync();
            return Ok(new { message = "Database reset successfully. Use /api/data/seed to populate with initial data." });
        }
    }
}