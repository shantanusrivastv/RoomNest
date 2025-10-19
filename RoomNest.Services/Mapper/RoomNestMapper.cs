using AutoMapper;
using RoomNest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity = RoomNest.Entities;
using model = RoomNest.Model;

namespace RoomNest.Services.Mapper
{
    public class RoomNestMapper : Profile
    {
        public RoomNestMapper()
        {
            CreateMap<entity.Hotel, model.HotelSearchResult>()
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

            CreateMap<entity.Room, model.Room>();
             
          
        }
    }
}
