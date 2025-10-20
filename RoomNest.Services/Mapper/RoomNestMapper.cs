using AutoMapper;
using RoomNest.DTO;
using RoomNest.Entities;

using entity = RoomNest.Entities;

namespace RoomNest.Services.Mapper
{
    public class RoomNestMapper : Profile
    {
        public RoomNestMapper()
        {
            CreateMap<entity.Hotel, HotelSearchResult>()
                    .ForMember(dest => dest.TotalRooms,
                    opt => opt.MapFrom(src => src.Rooms.Count))

                    .ForMember(dest => dest.RoomTypes,
                               opt => opt.MapFrom(src => src.Rooms
                    .GroupBy(r => r.RoomType)
                    .Select(g => new RoomTypeSummary
                    {
                        RoomType = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(rt => rt.RoomType)
                    .ToList()));

            CreateMap<entity.Room, RoomDto>();

            CreateMap<BookedRoom, RoomDto>();
            CreateMap<Booking, BookingResponse>();
        }
    }
}