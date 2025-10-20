using RoomNest.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomNest.Entities
{
    public class Room
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; } //FK
        public string RoomNumber { get; set; }
        public RoomType RoomType { get; set; }

        [NotMapped]
        public int Capacity => RoomType == RoomType.Single ? 1 : 2;

        //public decimal PricePerNight { get; set; }

        public virtual Hotel Hotel { get; set; } // Navigation property
        public virtual List<BookedRoom> BookedRoom { get; set; } = new();
    }
}