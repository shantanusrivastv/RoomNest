using AutoMapper;
using RoomNest.DTO;
using RoomNest.Infrastructure;

namespace RoomNest.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public HotelService(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<List<HotelSearchResult>> FindHotelByNameAsync(string name)
        {
            var res = await _hotelRepository.GetByNameAsync(name);

            var hotelSearchResults = _mapper.Map<List<HotelSearchResult>>(res);
            return hotelSearchResults;
        }

        public async Task<HotelSearchResult> FindHotelByIdAsync(int id)
        {
            var res = await _hotelRepository.GetByIdAsync(id);

            var hotelSearchResult = _mapper.Map<HotelSearchResult>(res);
            return hotelSearchResult;
        }
    }
}