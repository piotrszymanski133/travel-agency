using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsService.Models
{
    public class Hotel
    {
        public Hotel()
        {
            Hotelrooms = new List<Hotelroom>();
            Events = new List<Event>();
        }

        public short Id { get; set; } 
        public string Name { get; set; } = null!;
        public short? DestinationId { get; set; }
        public float Rating { get; set; }
        public string Food { get; set; } = null!;
        public short? Stars { get; set; }

        public virtual Destination? Destination { get; set; }
        public virtual List<Hotelroom> Hotelrooms { get; set; }
        public virtual List<Event> Events { get; set; }
    }
}