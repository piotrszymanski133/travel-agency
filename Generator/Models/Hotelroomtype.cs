using System;
using System.Collections.Generic;

namespace Generator
{
    public partial class Hotelroomtype
    {
        public Hotelroomtype()
        {
            Hotelrooms = new HashSet<Hotelroom>();
        }

        public short Id { get; set; }
        public string Name { get; set; } = null!;
        public short CapacityPeople { get; set; }

        public virtual ICollection<Hotelroom> Hotelrooms { get; set; }
    }
}
