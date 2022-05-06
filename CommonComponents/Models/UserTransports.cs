using System;

namespace CommonComponents.Models
{
    public class UserTransports
    {
        public Guid EventID { get; set; }
        public string TransportTypeName { get; set; }
        public int Persons { get; set; }
    }
}