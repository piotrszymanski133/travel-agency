using System;
using System.Collections.Generic;

namespace Generator
{
    public partial class Hotel
    {
        public Hotel()
        {
            Events = new HashSet<Event>();
            Hotelrooms = new HashSet<Hotelroom>();
        }

        public short Id { get; set; }
        public string Name { get; set; } = null!;
        public short DestinationId { get; set; }
        public float Rating { get; set; }
        public string Food { get; set; } = null!;
        public short? Stars { get; set; }

        public virtual Destination Destination { get; set; } = null!;
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Hotelroom> Hotelrooms { get; set; }
    }
}
