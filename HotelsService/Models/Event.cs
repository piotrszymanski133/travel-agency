using System;
using System.Collections.Generic;
using HotelsService.Models;

namespace HotelsService
{
    public partial class Event
    {
        public Event()
        {
            Eventrooms = new HashSet<Eventroom>();
        }

        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public string HotelId { get; set; } = null!;

        public virtual Hotel Hotel { get; set; } = null!;
        public DateTime? Creationtime { get; set; }

        public virtual ICollection<Eventroom> Eventrooms { get; set; }
    }
}
