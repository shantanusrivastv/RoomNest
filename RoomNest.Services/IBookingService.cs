using RoomNest.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Services
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(CreateBookingRequest req);
        Task<BookingResponse?> GetBookingByReferenceAsync(string bookingReference);
        Task<bool> CancelBookingAsync(string bookingReference);
    }
}
