using RoomNest.Entities;

namespace RoomNest.Infrastructure
{
    public class HotelRepository(RoomNestDbContext context) : BaseRepository<Hotel>(context), IHotelRepository
    {
        public async Task<List<Hotel>> GetByNameAsync(string name)
        {
            var res = await FindByAsync(x => x.Name.ToLower().Contains(name.ToLower()), true);
            return res.ToList();
        }

        public async Task<Hotel> GetByIdAsync(int id)
        {
            var res = await FindByAsync(x => x.HotelId == id, true);

            if (res != null && res.Count >= 1) //Todo refine it
            {
                return res.SingleOrDefault();
            }
            return null;
        }
    }
}