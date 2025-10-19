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
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateTime startDate, DateTime endDate, int numberOfGuests);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate);
    }
}
