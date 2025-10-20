using RoomNest.Entities;

namespace RoomNest.Infrastructure.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<Booking?> GetByReferenceAsync(string bookingReference);
        Task<bool> IsRoomBookedAsync(int roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate);
    }
}