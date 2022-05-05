using System;
using System.Collections.Generic;
using HotelsService.Models;

namespace HotelsService
{
    public partial class Event
    {
        public Event()
        {
            Eventrooms = new List<Eventroom>();
        }

        public Guid Id { get; set; }
        public Guid TripReservationId { get; set; }
        public string Type { get; set; } = null!;
        public string HotelId { get; set; } = null!;

        public virtual Hotel Hotel { get; set; } = null!;
        public DateTime? Creationtime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual List<Eventroom> Eventrooms { get; set; }
    }
}