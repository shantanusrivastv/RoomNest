using Microsoft.EntityFrameworkCore;
using RoomNest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Infrastructure
{
    public class HotelRepository(RoomNestDbContext context) : BaseRepository<Hotel>(context), IHotelRepository
    {

        public async Task<List<Hotel>> GetByNameAsync(string name)
        {
            var res =  await FindByAsync(x => x.Name.ToLower().Contains(name.ToLower()), true);      
            return res.ToList();
        }

        public Task<Hotel> GetByIdAsync(int id)
        {
            return Task.FromResult(FindBy(x => x.HotelId == id).SingleOrDefault());
        }

       
    }
}
