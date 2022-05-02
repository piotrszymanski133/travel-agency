using System.Collections.Generic;
using CommonComponents.Models;

namespace HotelsService.Models
{
    public class HotelStateOnDay
    { 
        public List<HotelRoom> FreeRooms { get; set; }

        public HotelStateOnDay()
        {
            FreeRooms = new List<HotelRoom>();
        }
    }
}