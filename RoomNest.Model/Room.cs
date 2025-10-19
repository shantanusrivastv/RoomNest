using RoomNest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RoomNest.Model
{
    public class Room
    {
        public int RoomId { get; set; }

        [JsonIgnore]
        public int HotelId { get; set; }
        public RoomType RoomType { get; set; }
        public int Capacity => RoomType == RoomType.Single ? 1 : 2;
    }
}
