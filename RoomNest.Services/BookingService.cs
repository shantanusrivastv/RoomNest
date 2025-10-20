using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RoomNest.Common;
using RoomNest.DTO;
using RoomNest.Entities;
using RoomNest.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoomNest.Services
{
    public class BookingService : IBookingService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingService(IHotelRepository hotelRepository,
                              IRoomRepository roomRepository,
                              IBookingRepository bookingRepository,
                              IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public Task<bool> CancelBookingAsync(string bookingReference)
        {
            //Tod Implement in future iteration
            throw new NotImplementedException();
        }

        public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest req)
        {
            var hotel = await _hotelRepository.GetByIdAsync(req.HotelId) ?? throw new InvalidOperationException($"Invalid hotel ID provided: {req.HotelId}");
            var requestedRooms = await _roomRepository.GetByIdAsync(req.RoomIds);

            if (requestedRooms.Count != req.RoomIds.Length)
            {
                throw new InvalidOperationException("One or more requested rooms do not exist");
            }

            var invalidRooms = requestedRooms.Where(r => r.HotelId != req.HotelId).ToList();
            if (invalidRooms.Any())
                throw new InvalidOperationException("One or more rooms do not belong to the specified hotel");

            var totalCapacity = requestedRooms.Sum(r => r.Capacity);

            if (totalCapacity < req.NumberOfGuests)
                throw new InvalidOperationException($"The available rooms do not have sufficient capacity to accommodate {req.NumberOfGuests} guests.");

            foreach (var room in requestedRooms)
            {
                var isBooked = await _bookingRepository.IsRoomBookedAsync(room.RoomId, req.CheckInDate, req.CheckOutDate);
                if (isBooked)
                    throw new InvalidOperationException($"Room {room.RoomId} is already booked for the selected dates");                   
            }   
             

            //Todo
            //var numberOfNights = (req.CheckOutDate - req.CheckInDate).Days;
            //var totalPrice = numberOfNights * room.PricePerNight;

            var booking = new Booking
            {
                BookingReference = GenerateBookingReference(),
                HotelId = req.HotelId,
                CheckInDate = req.CheckInDate,
                CheckOutDate = req.CheckOutDate,
                NumberOfGuests = req.NumberOfGuests,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTimeOffset.UtcNow,
                GuestName = req.Guest.GuestName,
                GuestEmail = req.Guest.GuestEmail,
                GuestPhone = req.Guest.GuestPhone                
            };

            booking.BookedRoom = requestedRooms.Select(r => new BookedRoom
            {
                RoomId = r.RoomId,
                Room = r
            }).ToList();

            var confirmedBooking = await _bookingRepository.AddAsync(booking);

            
                var bookingResponse = _mapper.Map<BookingResponse>(confirmedBooking);
                bookingResponse.Rooms = _mapper.Map<List<RoomDto>>(
                                            confirmedBooking.BookedRoom.Select(br => br.Room));
                return bookingResponse;
            
            
            
        }

        private static string GenerateBookingReference()
        {
            // Generate a unique booking reference (e.g., BK20231015123456789)
            var timestamp = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmssfff");
            return $"BK{timestamp}";
        }

        public async Task<BookingResponse?> GetBookingByReferenceAsync(string bookingReference)
        {
            var booking = await _bookingRepository.GetByReferenceAsync(bookingReference);
            if (booking is null)
                return null;

            var bookingResponse = _mapper.Map<BookingResponse>(booking);
            bookingResponse.Rooms = _mapper.Map<List<RoomDto>>(booking.BookedRoom.Select(br=> br.Room));
            return bookingResponse;
        }
    }
}
