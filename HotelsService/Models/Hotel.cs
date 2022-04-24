using System;
using System.Collections.Generic;

namespace HotelsService.Models
{
    public partial class Hotel
    {
        public Hotel()
        {
            Hotelrooms = new HashSet<Hotelroom>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public short? DestinationId { get; set; }
        public float Rating { get; set; }
        public string Food { get; set; } = null!;
        public short? Stars { get; set; }

        public virtual Destination? Destination { get; set; }
        public virtual ICollection<Hotelroom> Hotelrooms { get; set; }
    }
}
