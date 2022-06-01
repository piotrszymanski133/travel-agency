using System;
using System.Collections.Generic;

namespace Generator
{
    public partial class Hotelroom
    {
        public Hotelroom()
        {
            Hotelroomavailabilities = new HashSet<Hotelroomavailability>();
        }

        public short Id { get; set; }
        public short HotelId { get; set; }
        public short RoomtypeId { get; set; }

        public virtual Hotel Hotel { get; set; } = null!;
        public virtual Hotelroomtype Roomtype { get; set; } = null!;
        public virtual ICollection<Hotelroomavailability> Hotelroomavailabilities { get; set; }
    }
}
