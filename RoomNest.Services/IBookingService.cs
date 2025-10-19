using RoomNest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Services
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(Booking booking);
        Task<BookingResponse?> GetBookingByReferenceAsync(string reference);
        Task<bool> CancelBookingAsync(string reference);
    }
}
