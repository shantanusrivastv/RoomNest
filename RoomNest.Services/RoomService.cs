using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RoomNest.DTO;
using RoomNest.Entities;
using RoomNest.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<AvailabilityResponse> CheckAvailabilityAsync(AvailabilityRequest request)
        {
            //todo Input validation

            var (hotelName, availableRooms) = await _roomRepository.GetAvailableRoomsAsync(request.HotelId, 
                                                                                           request.CheckInDate,
                                                                                           request.CheckOutDate,
                                                                                           request.GuestCount);

            var roomDtos = _mapper.Map<List<RoomDto>>(availableRooms);

            return new AvailabilityResponse()
            {
                HotelName = hotelName ?? "Hotel not found",
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                RequestedGuestCount = request.GuestCount,
                AvailableRooms = roomDtos
            };

        }
    }
}
