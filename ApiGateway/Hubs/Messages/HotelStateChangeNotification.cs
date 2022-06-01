using System;

namespace ApiGateway.Hubs
{
    public class HotelStateChangeNotification
    {
        public int HotelId { get; set; }
        public short Change { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}