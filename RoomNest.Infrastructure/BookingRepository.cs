using Microsoft.EntityFrameworkCore;
using RoomNest.Common;
using RoomNest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Infrastructure
{
    public class BookingRepository(RoomNestDbContext context) : BaseRepository<Booking>(context), IBookingRepository
    {       

        public async Task<Booking?> GetByReferenceAsync(string bookingReference)
        {
            return await _context.Bookings.Include(b=> b.BookedRoom)
                                .ThenInclude(br=> br.Room)
                                .FirstOrDefaultAsync(b=> b.BookingReference == bookingReference);
        }

        public async Task<bool> IsRoomBookedAsync(int roomId, DateTimeOffset checkInDate, DateTimeOffset checkOutDate)
        {
            var bookingExist =  await FindByAsync(b=> b.Status == BookingStatus.Confirmed
                                              && b.CheckInDate < checkOutDate
                                              && b.CheckOutDate > checkInDate).AnyAsync(b => b.BookedRoom.Any(br => br.RoomId == roomId));
            return bookingExist;
        }
    }
}
