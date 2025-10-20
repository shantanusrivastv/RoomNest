using RoomNest.DTO;

namespace RoomNest.Services.Interfaces
{
    public interface IRoomService
    {
        Task<AvailabilityResponse> CheckAvailabilityAsync(AvailabilityRequest request);
    }
}