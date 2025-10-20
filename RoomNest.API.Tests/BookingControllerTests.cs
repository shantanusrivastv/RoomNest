using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using RoomNest.API.Controllers;
using RoomNest.Common;
using RoomNest.DTO;
using RoomNest.Services.Interfaces;

namespace RoomNest.API.Tests
{
    public class BookingControllerTests
    {
        private readonly IBookingService _bookingService = Substitute.For<IBookingService>();
        private readonly BookingController _controller;

        public BookingControllerTests()
        {
            _controller = new BookingController(_bookingService);
        }

        [Fact]
        public async Task InvalidCheckInAndCheckOutShouldReturnsBadRequest()
        {
            var request = new CreateBookingRequest
            {
                HotelId = 1,
                RoomIds = new[] { 101 },
                CheckInDate = DateTimeOffset.Parse("2025-12-10"),
                CheckOutDate = DateTimeOffset.Parse("2025-12-05"),
                NumberOfGuests = 1,
                Guest = new GuestInfo
                {
                    GuestName = "Alice",
                    GuestEmail = "alice@example.com",
                    GuestPhone = "+1234567890"
                }
            };

            var result = await _controller.CreateBooking(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Check-in date must be before check-out date", badRequest.Value);
        }

        [Fact]
        public async Task InvalidNumberOfGuestsWhileBookingShouldReturnsBadRequest()
        {
            var request = new CreateBookingRequest
            {
                HotelId = 2,
                RoomIds = new[] { 201 },
                CheckInDate = DateTimeOffset.UtcNow,
                CheckOutDate = DateTimeOffset.UtcNow.AddDays(1),
                NumberOfGuests = 0,
                Guest = new GuestInfo
                {
                    GuestName = "Bob",
                    GuestEmail = "bob@example.com",
                    GuestPhone = "+1987654321"
                }
            };

            var result = await _controller.CreateBooking(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Number of guests must be greater than zero", badRequest.Value);
        }

        [Fact]
        public async Task BookingShouldWorkForValidInput()
        {
            var now = DateTimeOffset.UtcNow;
            var request = new CreateBookingRequest
            {
                HotelId = 3,
                RoomIds = new[] { 301, 302 },
                CheckInDate = now,
                CheckOutDate = now.AddDays(2),
                NumberOfGuests = 2,
                Guest = new GuestInfo
                {
                    GuestName = "Charlie",
                    GuestEmail = "charlie@example.com",
                    GuestPhone = "+1122334455"
                }
            };

            var rooms = new List<RoomDto>
            {
                new() { RoomId = 301, HotelId = 3, RoomType = RoomType.Single },
                new() { RoomId = 302, HotelId = 3, RoomType = RoomType.Double }
            };

            var response = new BookingResponse
            {
                BookingReference = "BK-2025-0001",
                GuestName = request.Guest.GuestName,
                GuestEmail = request.Guest.GuestEmail,
                NumberOfGuests = request.NumberOfGuests,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                Rooms = rooms,
                CreatedAt = now
            };

            _bookingService
                .CreateBookingAsync(request)
                .Returns(Task.FromResult(response));

            var result = await _controller.CreateBooking(request);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(BookingController.GetBooking), created.ActionName);
            Assert.Equal(response.BookingReference, created.RouteValues?["reference"]);

            var actual = Assert.IsType<BookingResponse>(created.Value);
            Assert.Equal(response.BookingReference, actual.BookingReference);
            Assert.Equal(response.GuestName, actual.GuestName);
            Assert.Equal(response.GuestEmail, actual.GuestEmail);
            Assert.Equal(response.NumberOfGuests, actual.NumberOfGuests);
            Assert.Equal(response.CheckInDate, actual.CheckInDate);
            Assert.Equal(response.CheckOutDate, actual.CheckOutDate);
            Assert.Equal(response.CreatedAt, actual.CreatedAt);
            Assert.Equal(2, actual.Rooms.Count);
            Assert.Collection(actual.Rooms,
                r =>
                {
                    Assert.Equal(301, r.RoomId);
                    Assert.Equal(RoomType.Single, r.RoomType);
                    Assert.Equal(1, r.Capacity);
                },
                r =>
                {
                    Assert.Equal(302, r.RoomId);
                    Assert.Equal(RoomType.Double, r.RoomType);
                    Assert.Equal(2, r.Capacity);
                });
        }

        [Fact]
        public async Task InvalidBookingReferenceShouldReturnNotFound()
        {
            _bookingService?.GetBookingByReferenceAsync("MISSING")
                           .Returns(Task.FromResult<BookingResponse>(null));

            var result = await _controller.GetBooking("MISSING");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task ValidBookingReferenceShouldReturnBooking()
        {
            var now = DateTimeOffset.UtcNow;
            var rooms = new List<RoomDto>
            {
                new() { RoomId = 401, HotelId = 4, RoomType = RoomType.Deluxe }
            };
            var expected = new BookingResponse
            {
                BookingReference = "BK-2025-0002",
                GuestName = "Dana",
                GuestEmail = "dana@example.com",
                NumberOfGuests = 1,
                CheckInDate = now,
                CheckOutDate = now.AddDays(1),
                Rooms = rooms,
                CreatedAt = now
            };

            _bookingService?.GetBookingByReferenceAsync("BK-2025-0002")
                           .Returns(Task.FromResult(expected));

            var result = await _controller.GetBooking("BK-2025-0002");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var actual = Assert.IsType<BookingResponse>(ok.Value);
            Assert.Equal(expected.BookingReference, actual.BookingReference);
            Assert.Equal(expected.GuestName, actual.GuestName);
            Assert.Equal(expected.GuestEmail, actual.GuestEmail);
            Assert.Equal(expected.NumberOfGuests, actual.NumberOfGuests);
            Assert.Equal(expected.CheckInDate, actual.CheckInDate);
            Assert.Equal(expected.CheckOutDate, actual.CheckOutDate);
            Assert.Equal(expected.CreatedAt, actual.CreatedAt);
            Assert.Single(actual.Rooms);
            Assert.Equal(401, actual.Rooms[0].RoomId);
            Assert.Equal(RoomType.Deluxe, actual.Rooms[0].RoomType);
        }
    }
}