using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Entities
{
    public class Room
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; } //FK
        public string RoomNumber { get; set; }
        public RoomType RoomType { get; set; }

        //public int Capacity => RoomType == RoomType.Single ? 1 : 2;
        //public decimal PricePerNight { get; set; }
        
        public virtual Hotel Hotel { get; set; } // Navigation property
        public virtual List<Booking> Bookings { get; set; } = new();
    }

    public enum RoomType
    {
        Single,
        Double,
        Delux
    }
}
