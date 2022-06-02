using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Generator
{
    public interface IHotelRepository
    {
        public Hotel GetRandomHotel();
        public List<Event> GetEventsForDate(DateOnly date, int roomTypeId, int hotelId);

    }

    public class HotelRepository : IHotelRepository
    {

        public Hotel GetRandomHotel()
        {
            using var db = new hotelsContext();
            List<Hotel> hotels = db.Hotels
                .Include(h => h.Events)
                .Include(h => h.Destination)
                .Include(h => h.Hotelrooms)
                .ThenInclude(r => r.Roomtype)
                .Include(h => h.Hotelrooms)
                .ThenInclude(r => r.Hotelroomavailabilities)
                .ToList();
            
            Random rnd = new Random();
            //return hotels[rnd.Next(hotels.Count)];
            return hotels.Find(h => h.Id==1);

        }

        public List<Event> GetEventsForDate(DateOnly date, int roomTypeId, int hotelId)
        {
            using var db = new hotelsContext();
            return db.Events.Where(ev =>
                    ev.HotelId == hotelId && 
                    ev.RoomtypeId == roomTypeId && 
                    ev.Startdate <= date && 
                    ev.Enddate >= date)
                .ToList();
        }
    }
}