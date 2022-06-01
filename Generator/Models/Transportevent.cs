using System;
using System.Collections.Generic;

namespace Generator
{
    public partial class Transportevent
    {
        public Guid Id { get; set; }
        public long TransportId { get; set; }
        public Guid EventId { get; set; }
        public int Places { get; set; }
        public string Type { get; set; } = null!;
        public string Username { get; set; } = null!;

        public virtual Transport Transport { get; set; } = null!;
    }
}
