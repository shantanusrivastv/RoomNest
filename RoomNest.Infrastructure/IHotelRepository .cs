using RoomNest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Infrastructure
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<List<Hotel>> GetByNameAsync(string name);
        Task<Hotel> GetByIdAsync(int id);
    }
}
