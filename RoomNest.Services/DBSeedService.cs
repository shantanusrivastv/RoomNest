using Microsoft.EntityFrameworkCore;
using RoomNest.Common;
using RoomNest.Entities;
using RoomNest.Infrastructure;

namespace RoomNest.Services
{
    public class DBSeedService : IDBSeedService
    {
        private readonly RoomNestDbContext _context;

        public DBSeedService(
             RoomNestDbContext context)
        {
            _context = context;
        }
        public async Task SeedAsync()
        {
            // Check if data already exists
            if (await  _context.Hotels.AnyAsync())
            {
                return; // Database already seeded
            }

            // Create hotel
            var hotel1 = new Entities.Hotel
            {
                Name = "Grand Plaza Hotel",
                Address = "123 Main Street",
                City = "London",
                Country = "United Kingdom",
                ContactEmail = "info@grandplaza.com",
                ContactPhone = "+44-20-12345678"
            };

            var hotel2 = new Entities.Hotel
            {
                Name = "Lakeview  Hotel",
                Address = "45 Orchard Road",
                City = "Oxford",
                Country = "United Kingdom",
                ContactEmail = "info@lakeview.com",
                ContactPhone = "+44-20-12345671"
            };

            var hotel3 = new Entities.Hotel
            {
                Name = "Grand Central",
                Address = "789 Central Avenue",
                City = "Manchester",
                Country = "United Kingdom",
                ContactEmail = "info@grandcentral.com",
                ContactPhone = "+44-20-12345672"
            };

            await _context.Hotels.AddRangeAsync(hotel1, hotel2, hotel3);
            await _context.SaveChangesAsync(); // Ensure HotelIds are generated

            //  rooms for Hotel1
            var roomsForHotel1 = new List<Room>
        {
            new Room { HotelId = hotel1.HotelId, RoomNumber = "101", RoomType = RoomType.Single },
            new Room { HotelId = hotel1.HotelId, RoomNumber = "102", RoomType = RoomType.Single },
            new Room { HotelId = hotel1.HotelId, RoomNumber = "201", RoomType = RoomType.Double },
            new Room { HotelId = hotel1.HotelId, RoomNumber = "202", RoomType = RoomType.Double },
            new Room { HotelId = hotel1.HotelId, RoomNumber = "301", RoomType = RoomType.Deluxe }
        };

            //  rooms for Hotel2
            var roomsForHotel2 = new List<Room>
        {
            new Room { HotelId = hotel2.HotelId, RoomNumber = "A1", RoomType = RoomType.Single },
            new Room { HotelId = hotel2.HotelId, RoomNumber = "A2", RoomType = RoomType.Single },
            new Room { HotelId = hotel2.HotelId, RoomNumber = "B1", RoomType = RoomType.Double },
            new Room { HotelId = hotel2.HotelId, RoomNumber = "B2", RoomType = RoomType.Double },
            new Room { HotelId = hotel2.HotelId, RoomNumber = "C1", RoomType = RoomType.Deluxe }
        };

            //  rooms for Hotel3
            var roomsForHotel3 = new List<Room>
    {
        new Room { HotelId = hotel3.HotelId, RoomNumber = "100", RoomType = RoomType.Deluxe },
        new Room { HotelId = hotel3.HotelId, RoomNumber = "101", RoomType = RoomType.Deluxe },
        new Room { HotelId = hotel3.HotelId, RoomNumber = "200", RoomType = RoomType.Deluxe },
        new Room { HotelId = hotel3.HotelId, RoomNumber = "201", RoomType = RoomType.Deluxe },
        new Room { HotelId = hotel3.HotelId, RoomNumber = "300", RoomType = RoomType.Deluxe },
        new Room { HotelId = hotel3.HotelId, RoomNumber = "301", RoomType = RoomType.Double }
    };

            await _context.Rooms.AddRangeAsync(roomsForHotel1);
            await _context.Rooms.AddRangeAsync(roomsForHotel2);
            await _context.Rooms.AddRangeAsync(roomsForHotel3);
            await _context.SaveChangesAsync();
        }

        public async Task ResetAsync()
        {
            // Remove all data
            _context.BookedRoom.RemoveRange(_context.BookedRoom);
            _context.Bookings.RemoveRange(_context.Bookings);
            _context.Rooms.RemoveRange(_context.Rooms);            
            _context.Hotels.RemoveRange(_context.Hotels);           

            await _context.SaveChangesAsync();
        }
    }
}
