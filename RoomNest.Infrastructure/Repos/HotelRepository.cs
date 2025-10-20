using RoomNest.Entities;
using RoomNest.Infrastructure.DBContext;
using RoomNest.Infrastructure.Interfaces;
using System.Threading;

namespace RoomNest.Infrastructure.Repos
{
    public class HotelRepository(RoomNestDbContext context) : BaseRepository<Hotel>(context), IHotelRepository
    {
        public async Task<List<Hotel>?> GetByNameAsync(string name)
        {
            var res = await FindByAsync(h => h.Name.ToLower().Contains(name.ToLower()), true);
            return res?.ToList();
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            var results = await FindByAsync(x => x.HotelId == id, true);
            return results?.SingleOrDefault();
        }
    }
}