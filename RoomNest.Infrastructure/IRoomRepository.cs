using RoomNest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Infrastructure
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<(string HotelName, IEnumerable<Room>)> GetAvailableRoomsAsync(int hotelId, DateTimeOffset checkInDate,
                                                        DateTimeOffset checkOutDate, int numberOfGuests);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate);
    }
}
