using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ApiGateway.Models
{
    public class TripParameters
    {
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Adults { get; set; }
        public int ChildrenUnder3 { get; set; }
        public int ChildrenUnder10 { get; set; }
        public int ChildrenUnder18 { get; set; }
    }
}