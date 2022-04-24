using System;
using System.Collections.Generic;

namespace HotelsService.Models
{
    public partial class Destination
    {
        public Destination()
        {
            Hotels = new HashSet<Hotel>();
        }

        public short Id { get; set; }
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;

        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
