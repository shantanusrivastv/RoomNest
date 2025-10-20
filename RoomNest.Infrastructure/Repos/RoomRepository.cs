using Microsoft.EntityFrameworkCore;
using RoomNest.Common;
using RoomNest.Entities;
using RoomNest.Infrastructure.DBContext;
using RoomNest.Infrastructure.Interfaces;

namespace RoomNest.Infrastructure.Repos
{
    public class RoomRepository(RoomNestDbContext context) : BaseRepository<Room>(context), IRoomRepository
    {
        public async Task<(string HotelName, IEnumerable<Room>)> GetAvailableRoomsAsync(int hotelId, DateTimeOffset checkInDate,
                                                                    DateTimeOffset checkOutDate, int numberOfGuests)
        {
            var bookedRoom = await _context.Bookings.Where(b => b.HotelId == hotelId
                                                && b.Status == BookingStatus.Confirmed
                                                && b.CheckInDate < checkOutDate
                                                && b.CheckOutDate > checkInDate)
                                            .ToListAsync();

            var roomIds = bookedRoom.SelectMany(b => b.BookedRoom.Select(br => br.RoomId)).ToList(); //Todo move up used for debugging

            var availableRooms = FindBy(r => r.HotelId == hotelId && !roomIds.Contains(r.RoomId)).AsNoTracking().ToList();
            var totalCapacity = availableRooms.Sum(r => r.Capacity);

            if (totalCapacity < numberOfGuests)
                return (null, Enumerable.Empty<Room>()); // not enough capacity

            var hotelName = await _context.Hotels.Where(h => h.HotelId == hotelId)
                                                 .Select(h => h.Name)
                                                 .FirstOrDefaultAsync();

            return (hotelName, availableRooms);
        }

        public async Task<List<Room>?> GetByIdAsync(int[] roomIds)
        {
            var res = await FindByAsync(r => roomIds.Contains(r.RoomId), false);            
            return res?.ToList();
        }
    }
}