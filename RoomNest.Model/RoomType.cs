using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNest.Model
{
    public enum RoomType
    {
        Single,
        Double,
        Deluxe
    }

    public enum BookingStatus
    {
        Confirmed,
        Cancelled,
        CheckedIn,
        CheckedOut
    }
}
