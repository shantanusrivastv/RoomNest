using RoomNest.Model;

namespace RoomNest.Services
{
    public interface IRoomService
    {
        Task<AvailabilityResponse> CheckAvailabilityAsync(AvailabilityRequest request);
    }
}