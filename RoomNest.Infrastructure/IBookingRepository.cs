using RoomNest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Infrastructure
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<Booking?> GetByReferenceAsync(string bookingReference);
        Task<bool> IsRoomBookedAsync(int roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate);
    }
}
