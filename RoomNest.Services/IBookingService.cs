using RoomNest.DTO;

namespace RoomNest.Services
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(CreateBookingRequest req);

        Task<BookingResponse?> GetBookingByReferenceAsync(string bookingReference);

        //Task<bool> CancelBookingAsync(string bookingReference); //Not required at the moment
    }
}