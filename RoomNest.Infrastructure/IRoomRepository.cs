using RoomNest.Entities;

namespace RoomNest.Infrastructure
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<(string HotelName, IEnumerable<Room>)> GetAvailableRoomsAsync(int hotelId, DateTimeOffset checkInDate,
                                                        DateTimeOffset checkOutDate, int numberOfGuests);

        Task<List<Room>> GetByIdAsync(int[] roomIds);
    }
}