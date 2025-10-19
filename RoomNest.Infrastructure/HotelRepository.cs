using Microsoft.EntityFrameworkCore;
using RoomNest.Common;
using RoomNest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
