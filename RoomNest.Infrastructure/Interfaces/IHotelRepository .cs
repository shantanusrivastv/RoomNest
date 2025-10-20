using RoomNest.Entities;

namespace RoomNest.Infrastructure.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<List<Hotel>> GetByNameAsync(string name);
        Task<Hotel> GetByIdAsync(int id);
    }
}