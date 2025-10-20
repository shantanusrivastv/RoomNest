using AutoMapper;
using NSubstitute;
using RoomNest.Common;
using RoomNest.DTO;
using RoomNest.Entities;
using RoomNest.Infrastructure.Interfaces;
using RoomNest.Services.Implementations;

namespace RoomNest.Services.Tests
{
    public class BookingServiceTests
    {
        private readonly IHotelRepository _hotelRepo = Substitute.For<IHotelRepository>();
        private readonly IRoomRepository _roomRepo = Substitute.For<IRoomRepository>();
        private readonly IBookingRepository _bookingRepo = Substitute.For<IBookingRepository>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly BookingService _service;

        public BookingServiceTests()
        {
            _service = new BookingService(_hotelRepo, _roomRepo, _bookingRepo, _mapper);
        }

        [Fact]
        public async Task InvalidHotelInputShouldThrowsInvalidOperationException()
        {
            _hotelRepo.GetByIdAsync(Arg.Any<int>()).Returns(async _ => await Task.FromResult<Hotel?>(null));

            var req = new CreateBookingRequest { HotelId = 99, RoomIds = new[] { 1 }, CheckInDate = DateTimeOffset.UtcNow, CheckOutDate = DateTimeOffset.UtcNow.AddDays(1), NumberOfGuests = 1, Guest = new GuestInfo() };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateBookingAsync(req));
            Assert.Contains("Invalid hotel ID provided: 99", ex.Message);
        }

        [Fact]
        public async Task MissingRoomsWhileBookingShouldThrowsInvalidOperationException()
        {
            _hotelRepo.GetByIdAsync(1).Returns(new Hotel());
            //_roomRepo.GetByIdAsync(new[] { 101, 102 })
            //        .Returns(new List<Room> {
            //            new Room {  RoomId = 101, HotelId = 1}
            //    });

            _roomRepo.GetByIdAsync(Arg.Is<int[]>(ids => ids.SequenceEqual(new[] { 101, 102 })))
                  .Returns(new List<Room> {
                        new Room { RoomId = 101, HotelId = 1, RoomType = RoomType.Single }
                  });

            var req = new CreateBookingRequest
            {
                HotelId = 1,
                RoomIds = new[] { 101, 102 },
                CheckInDate = DateTimeOffset.UtcNow,
                CheckOutDate = DateTimeOffset.UtcNow.AddDays(1),
                NumberOfGuests = 1,
                Guest = new GuestInfo()
                {
                    GuestName = "Jane Smith",
                    GuestEmail = "jane@gmail.com",
                    GuestPhone = "+44 7448254567"
                }
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateBookingAsync(req));
            Assert.Equal("One or more requested rooms do not exist", ex.Message);
        }

        [Fact]
        public async Task InsufficientCapacityWhileBookingShouldThrowsInvalidOperationException()
        {
            _hotelRepo.GetByIdAsync(1).Returns(new Hotel());
            var rooms = new List<Room>
            {
                new Room { RoomId = 101, HotelId = 1, RoomType = RoomType.Single },
                new Room { RoomId = 102, HotelId = 1, RoomType = RoomType.Single }
            };
            _roomRepo.GetByIdAsync(new[] { 101, 102 }).Returns(rooms);

            _roomRepo.GetByIdAsync(Arg.Is<int[]>(ids => ids.SequenceEqual(new[] { 101, 102 })))
                    .Returns(rooms);

            var req = new CreateBookingRequest { HotelId = 1, RoomIds = new[] { 101, 102 }, CheckInDate = DateTimeOffset.UtcNow, CheckOutDate = DateTimeOffset.UtcNow.AddDays(1), NumberOfGuests = 3, Guest = new GuestInfo() };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateBookingAsync(req));
            Assert.Contains("sufficient capacity to accommodate 3 guests", ex.Message);
        }


        [Fact]
        public async Task CreateBookingShouldWorkWithValidRequest()
        {
            var now = DateTimeOffset.UtcNow;
            var req = new CreateBookingRequest
            {
                HotelId = 1,
                RoomIds = new[] { 101 },
                CheckInDate = now,
                CheckOutDate = now.AddDays(1),
                NumberOfGuests = 1,
                Guest = new GuestInfo { GuestName = "John", GuestEmail = "j@example.com", GuestPhone = "123" }
            };

            _hotelRepo.GetByIdAsync(1).Returns(new Hotel { HotelId = 1 });

            var room = new Room { RoomId = 101, HotelId = 1, RoomType = RoomType.Double };

            _roomRepo.GetByIdAsync(Arg.Is<int[]>(ids => ids.SequenceEqual(new[] { 101 })))
                    .Returns(new List<Room> { room });

            _bookingRepo.IsRoomBookedAsync(101, now, now.AddDays(1)).Returns(false);

            var saved = new Booking
            {
                BookingReference = "BK123",
                HotelId = 1,
                CheckInDate = now,
                CheckOutDate = now.AddDays(1),
                NumberOfGuests = 1,
                Status = BookingStatus.Confirmed,
                CreatedAt = now,
                GuestName = "John",
                GuestEmail = "j@example.com",
                GuestPhone = "123",
                BookedRoom = new List<BookedRoom> { new BookedRoom { Room = room } }
            };

            _bookingRepo.AddAsync(Arg.Any<Booking>()).Returns(Task.FromResult(saved));

            var dto = new BookingResponse { BookingReference = "BK123" };
            _mapper.Map<BookingResponse>(saved).Returns(dto);
            _mapper.Map<List<RoomDto>>(Arg.Any<IEnumerable<Room>>()).Returns(new List<RoomDto>());

            var result = await _service.CreateBookingAsync(req);

            Assert.Equal("BK123", result.BookingReference);
            await _bookingRepo.Received().AddAsync(Arg.Is<Booking>(b => b.HotelId == 1 && b.GuestName == "John"));
            _mapper.Received().Map<BookingResponse>(saved);
            _mapper.Received().Map<List<RoomDto>>(Arg.Is<IEnumerable<Room>>(r => r.Single().RoomId == 101));
        }

        [Fact]
        public async Task InvalidBookingRefrenceShouldReturnNull()
        {
            _bookingRepo.GetByReferenceAsync("X").Returns(Task.FromResult<Booking?>(null));
            var result = await _service.GetBookingByReferenceAsync("X");
            Assert.Null(result);
        }

        [Fact]
        public async Task ValidBookingRefrenceShouldReturnBooking()
        {
            var booking = new Booking
            {
                BookingReference = "BK999",
                BookedRoom = new List<BookedRoom> { new BookedRoom { Room = new Room { RoomId = 202 } } }
            };

            _bookingRepo.GetByReferenceAsync("BK999")
                        .Returns(Task.FromResult<Booking?>(booking));

            var dto = new BookingResponse { BookingReference = "BK999" };
            _mapper.Map<BookingResponse>(booking).Returns(dto);
            _mapper.Map<List<RoomDto>>(Arg.Any<IEnumerable<Room>>()).Returns(new List<RoomDto>());

            var result = await _service.GetBookingByReferenceAsync("BK999");

            Assert.NotNull(result);
            Assert.Equal("BK999", result!.BookingReference);
            _mapper.Received().Map<BookingResponse>(booking);
            _mapper.Received().Map<List<RoomDto>>(Arg.Is<IEnumerable<Room>>(rs => rs.Single().RoomId == 202));
        }
    }
}
