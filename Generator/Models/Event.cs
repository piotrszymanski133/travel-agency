using System;
using System.Collections.Generic;

namespace Generator
{
    public partial class Event
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public short RoomtypeId { get; set; }
        public Guid TripreservationId { get; set; }
        public string Type { get; set; } = null!;
        public short HotelId { get; set; }
        public DateOnly Startdate { get; set; }
        public DateOnly Enddate { get; set; }

        public virtual Hotel Hotel { get; set; } = null!;
    }
}
