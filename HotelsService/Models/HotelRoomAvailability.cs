using System;

namespace HotelsService.Models
{
    public class HotelRoomAvailability
    {
        public short Id { get; set; }
        public short HotelRoomId { get; set; }
        public short Quantity { get; set; }
        public DateTime Date { get; set; }
        public Hotelroom Hotelroom { get; set; }
    }
}