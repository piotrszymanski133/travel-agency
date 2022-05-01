using System;
using System.Collections.Generic;
using HotelsService.Models;

namespace HotelsService
{
    public partial class Hotelroom
    {
        public short Id { get; set; }
        public string HotelId { get; set; } = null!;
        public short RoomtypeId { get; set; }
        public short Quantity { get; set; }

        public virtual Hotel Hotel { get; set; } = null!;
        public virtual Hotelroomtype Roomtype { get; set; } = null!;
    }
}
