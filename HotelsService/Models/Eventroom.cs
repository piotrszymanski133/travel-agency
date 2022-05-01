using System;
using System.Collections.Generic;

namespace HotelsService
{
    public partial class Eventroom
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public short Quantity { get; set; }
        public short RoomtypeId { get; set; }

        public virtual Event Event { get; set; } = null!;
    }
}