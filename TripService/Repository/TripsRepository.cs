using System.Collections.Generic;
using System.Linq;
using CommonComponents;
using CommonComponents.Models;

namespace TripService.Repository
{
    public interface ITripsRepository
    {
        List<UserTrips> GetUserTrips(string Username);
        void SetUserTrip(CreateUserTripQuery queryParameters);
    }
    
    public class TripsRepository: ITripsRepository
    {
        public List<UserTrips> GetUserTrips(string Username)
        {
            using var context = new tripsContext();

            var result = context.Orderedtrips.Where(x => x.Username == Username).ToList();
            List<UserTrips> returnList = new();

            foreach (var orderedtrip in result)
            {
                returnList.Add(new UserTrips()
                {
                    City = orderedtrip.City,
                    Country = orderedtrip.Country,
                    EndDate = orderedtrip.EndDate,
                    FoodType = orderedtrip.Food,
                    HotelName = orderedtrip.HotelName,
                    id = orderedtrip.TripId,
                    Persons = orderedtrip.Persons,
                    StartDate = orderedtrip.StartDate,
                    TransportTypeName = orderedtrip.TransportTypeName,
                    HotelRoomName = orderedtrip.RoomTypeName
                    
                });
            }

            return returnList;
        }

        public void SetUserTrip(CreateUserTripQuery queryParameters)
        {
            using var context = new tripsContext();

            var obj = new Orderedtrip()
            {
                TripId = queryParameters.id,
                City = queryParameters.City,
                Country = queryParameters.Country,
                Food = queryParameters.FoodTypeName,
                Persons = queryParameters.Persons,
                RoomTypeName = queryParameters.RoomTypeName,
                TransportTypeName = queryParameters.TransportTypeName,
                Username = queryParameters.username,
                StartDate = queryParameters.StartDate.AddHours(4).ToUniversalTime(),
                EndDate = queryParameters.EndDate.AddHours(4).ToUniversalTime(),
                HotelName = queryParameters.HotelName
            };
            context.Orderedtrips.Add(obj);
            context.SaveChanges();
        }
    }
}