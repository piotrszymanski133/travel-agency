using System;
using System.Collections.Generic;

namespace CommonComponents.Models
{
    
    public class HotelOffer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DestinationCountry { get; set; }
        public string DestinationCity { get; set; }
        public float Rating { get; set; }
        public string Food { get; set; }
        public short Stars { get; set; }
        public string Description { get; set; }
        public List<HotelRoom> RoomsConfigurations { get; set; }
    }
    

}