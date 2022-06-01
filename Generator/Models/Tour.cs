using System;
using System.Collections.Generic;

namespace Generator
{
    public partial class Tour
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public float Rating { get; set; }
        public string Food { get; set; } = null!;
    }
}
