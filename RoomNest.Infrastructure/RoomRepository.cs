using Microsoft.EntityFrameworkCore;
using RoomNest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Infrastructure
{
    public class RoomRepository(RoomNestDbContext context) : BaseRepository<Room>(context), IRoomRepository
    {
        public async Task<(string HotelName, IEnumerable<Room>)> GetAvailableRoomsAsync(int hotelId, DateTimeOffset checkInDate,
                                                                    DateTimeOffset checkOutDate, int numberOfGuests)
        {
            var bookedRoomIds = await _context.Bookings.Where(b => b.HotelId == hotelId
                                                && b.Status == BookingStatus.Confirmed
                                                && b.CheckInDate < checkOutDate
                                                && b.CheckOutDate > checkInDate)
                                            .ToListAsync();

            var roomIds = bookedRoomIds.Select(r => r.RoomId).ToList(); //Todo move up used for debugging

            var availableRooms = FindBy(r => r.HotelId == hotelId  &&  !roomIds.Contains(r.RoomId)).AsNoTracking().ToList();
            var totalCapacity = availableRooms.Sum(r => r.Capacity);

            if (totalCapacity < numberOfGuests)
                return (null, Enumerable.Empty<Room>()); // not enough capacity

            var hotelName = await _context.Hotels.Where(h => h.HotelId == hotelId)
                                                 .Select(h => h.Name)
                                                 .FirstOrDefaultAsync();

            return (hotelName, availableRooms);
        }

        public Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}

