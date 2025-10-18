using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Model
{
    public class Room
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public int RoomNumber { get; set; } 
        public RoomType Type { get; set; }
        public int Capacity => Type == RoomType.Single ? 1 : 2;
    }
}
