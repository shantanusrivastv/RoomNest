using RoomNest.Model;

namespace RoomNest.Services
{
    public interface IHotelService
    {
        Task<List<HotelSearchResult>> FindHotelByNameAsync(string name);
        Task<HotelSearchResult> FindHotelByIdAsync(int id);
    }
}