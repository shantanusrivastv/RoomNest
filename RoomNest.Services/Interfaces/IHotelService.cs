using RoomNest.DTO;

namespace RoomNest.Services.Interfaces
{
    public interface IHotelService
    {
        Task<List<HotelSearchResult>> FindHotelByNameAsync(string name);
        Task<HotelSearchResult> FindHotelByIdAsync(int id);
    }
}