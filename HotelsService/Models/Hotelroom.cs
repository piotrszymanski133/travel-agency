using System;
using System.Collections.Generic;

namespace HotelsService.Models
{
    public partial class Hotelroom
    {
        public short Id { get; set; }
        public string HotelId { get; set; } = null!;
        public short CapacityPeople { get; set; }
        public short Quantity { get; set; }

        public virtual Hotel Hotel { get; set; } = null!;
    }
}
