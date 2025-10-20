using RoomNest.Entities;

namespace RoomNest.Infrastructure
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<Booking?> GetByReferenceAsync(string bookingReference);
        Task<bool> IsRoomBookedAsync(int roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate);
    }
}