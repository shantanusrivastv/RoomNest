using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RoomNest.Common;
using RoomNest.Entities;
using RoomNest.Infrastructure.DBContext;
using RoomNest.Infrastructure.Repos;

namespace RoomNest.Infrastructure.Tests
{
    public class BookingRepositoryTests
    {
        private readonly DbContextOptions<RoomNestDbContext> _dbOptions;

        public BookingRepositoryTests()
        {
            // Unique in-memory database for each test
            _dbOptions = new DbContextOptionsBuilder<RoomNestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private RoomNestDbContext GetContext()
        {
            var ctx = new RoomNestDbContext(_dbOptions);

            // Seed Hotels
            ctx.Hotels.Add(new Hotel
            {
                HotelId = 1,
                Name = "Test Hotel",
                Address = "123 Test St",
                City = "Test City",
                Country = "Test Country",
                ContactEmail = "contact@test.com",
                ContactPhone = "555-1234"
            });

            // Seed Rooms
            ctx.Rooms.AddRange(new[]
            {
                    new Room
                    {
                        RoomId = 101,
                        HotelId = 1,
                        RoomNumber = "101A",
                        RoomType = RoomType.Single
                    },
                    new Room
                    {
                        RoomId = 102,
                        HotelId = 1,
                        RoomNumber = "102B",
                        RoomType = RoomType.Double
                    }
            });

            // Seed existing booking: Room 101 already booked for 2025-10-20 to 2025-10-22
            var booking = new Booking
            {
                BookingId = 1,
                BookingReference = "EXIST123",
                HotelId = 1,
                CheckInDate = new DateTimeOffset(2025, 10, 20, 0, 0, 0, TimeSpan.Zero),
                CheckOutDate = new DateTimeOffset(2025, 10, 22, 0, 0, 0, TimeSpan.Zero),
                NumberOfGuests = 1,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTimeOffset.UtcNow,
                GuestName = "Existing Guest",
                GuestEmail = "existing@example.com",
                GuestPhone = "123456"
            };
            booking.BookedRoom = new List<BookedRoom>
            {
                    new() { Booking = booking, RoomId = 101 }
            };

            ctx.Bookings.Add(booking);
            ctx.SaveChanges();

            return ctx;
        }

        [Fact]
        public async Task IsRoomBookedAsync_OverlappingDates_ReturnsTrue()
        {
            // Arrange
            await using var ctx = GetContext();
            var repo = new BookingRepository(ctx);

            // Act: check overlap with existing booking
            var isBooked = await repo.IsRoomBookedAsync(
                roomId: 101,
                checkInDate: new DateTimeOffset(2025, 10, 21, 0, 0, 0, TimeSpan.Zero),
                checkOutDate: new DateTimeOffset(2025, 10, 23, 0, 0, 0, TimeSpan.Zero)
            );

            // Assert
            isBooked.Should().BeTrue();
        }

        [Fact]
        public async Task IsRoomBookedShouldBeFalse()
        {
            // Arrange
            await using var ctx = GetContext();
            var repo = new BookingRepository(ctx);

            // Act: before existing booking window
            var before = await repo.IsRoomBookedAsync(
                roomId: 101,
                checkInDate: new DateTimeOffset(2025, 10, 18, 0, 0, 0, TimeSpan.Zero),
                checkOutDate: new DateTimeOffset(2025, 10, 20, 0, 0, 0, TimeSpan.Zero)
            );
            // Act: after existing booking window
            var after = await repo.IsRoomBookedAsync(
                roomId: 101,
                checkInDate: new DateTimeOffset(2025, 10, 22, 0, 0, 0, TimeSpan.Zero),
                checkOutDate: new DateTimeOffset(2025, 10, 24, 0, 0, 0, TimeSpan.Zero)
            );

            // Assert
            before.Should().BeFalse();
            after.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnSavedBooking()
        {
            // Arrange
            await using var ctx = GetContext();
            var bookingRepo = new BookingRepository(ctx);

            var newBooking = new Booking
            {
                HotelId = 1,
                BookingReference = "NEW123",
                CheckInDate = DateTimeOffset.Parse("2025-11-01T00:00:00Z"),
                CheckOutDate = DateTimeOffset.Parse("2025-11-03T00:00:00Z"),
                NumberOfGuests = 2,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTimeOffset.UtcNow,
                GuestName = "New Guest",
                GuestEmail = "new@example.com",
                GuestPhone = "987654"
            };
            newBooking.BookedRoom = new List<BookedRoom>
            {
                new() { RoomId = 102, Booking = newBooking }
            };

            // Act
            var saved = await bookingRepo.AddAsync(newBooking);
            var booking = await bookingRepo.GetByReferenceAsync("NEW123");

            // Assert
            booking.Should().NotBeNull();
            booking?.BookingReference.Should().Be("NEW123");
            booking?.BookedRoom.Should().ContainSingle(br => br.RoomId == 102);
        }

        [Fact]
        public async Task NonExistingBookingShouldReturnNull()
        {
            // Arrange
            await using var ctx = GetContext();
            var repo = new BookingRepository(ctx);

            // Act
            var result = await repo.GetByReferenceAsync("UNKNOWN");

            // Assert
            result.Should().BeNull();
        }
    }
}