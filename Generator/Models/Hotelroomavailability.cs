using System;
using System.Collections.Generic;

namespace Generator
{
    public partial class Hotelroomavailability
    {
        public short Id { get; set; }
        public short HotelroomId { get; set; }
        public short Quantity { get; set; }
        public DateOnly Date { get; set; }

        public virtual Hotelroom Hotelroom { get; set; } = null!;
    }
}
