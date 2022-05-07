using System;
using System.Collections.Generic;
using HotelsService.Models;

namespace HotelsService
{
    public partial class Event
    {

        public Guid Id { get; set; }
        public Guid TripReservationId { get; set; }
        public short RoomTypeId { get; set; }
        public string Username { get; set; }
        public string Type { get; set; } = null!;
        public short HotelId { get; set; }
        public virtual Hotel Hotel { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}