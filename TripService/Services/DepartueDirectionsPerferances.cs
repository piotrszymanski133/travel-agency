using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Configuration;
using CommonComponents;

namespace TripService.Services
{
    public class CustomDireciornComparator : IComparer<PurchaseDirectionEvents>
    {
        public int Compare(PurchaseDirectionEvents x, PurchaseDirectionEvents y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return x.EventDate.CompareTo(y.EventDate);
        }
    }
    public class CustomComparator : IComparer<PurchasePreferencesEvents>
    {
        public int Compare(PurchasePreferencesEvents x, PurchasePreferencesEvents y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return x.EventDate.CompareTo(y.EventDate);
        }
    }

    public interface IDepartueDirectionsPerferances
    {
        public void AddDirectionEvent(PurchaseDirectionEvents pEvent);
        public void AddPreferencesEvent(PurchasePreferencesEvents pEvent);
        public void GetTopDirectionPerference();
        public string GetPerferences();
    }

    public class DepartueDirectionsPerferances: IDepartueDirectionsPerferances
    {
        private IPublishEndpoint _publishEndpoint;
        private SortedSet<PurchaseDirectionEvents> _eventsSet;
        private SortedSet<PurchasePreferencesEvents> _preferencessSet;
        
        private string _popularCountry = string.Empty;
        private int _popularCountryCount = 0;
        
        private string _popularHotel = string.Empty;
        private int _popularHotelCount = 0;
        
        private string _popularRoom = string.Empty;
        private int _popularRoomCount = 0;
        
        private string _popularTransport = string.Empty;
        private int _popularTransportCount = 0;

        public DepartueDirectionsPerferances(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            _eventsSet = new SortedSet<PurchaseDirectionEvents>(new CustomDireciornComparator());
            _preferencessSet = new SortedSet<PurchasePreferencesEvents>(new CustomComparator());
        }

        public void AddDirectionEvent(PurchaseDirectionEvents pEvent)
        {
            _eventsSet.Add(pEvent);

            if (_eventsSet.Count >= 10)
            {
                _eventsSet.Remove(_eventsSet.Last());
            }

            GetTopDirectionPerference();

        }

        public void AddPreferencesEvent(PurchasePreferencesEvents pEvent)
        {
            _preferencessSet.Add(pEvent);

            if (_preferencessSet.Count >= 10)
            {
                _preferencessSet.Remove(_preferencessSet.Last());
            }

            GetTopHotelPerference();
        }

        private void GetTopHotelPerference()
        {
            var isChanged = false;
            
            //Hotel
            
            var result = _preferencessSet.GroupBy(item => item.HotelName).Select(grp =>
                new {
                    NameFiled = grp.Key, 
                    Occurences = grp.Count()
                }).OrderByDescending(x => x.Occurences).ToList();

            var findResult = result.Find(x => x.NameFiled == _popularHotel);
            
            if (findResult== null || _popularHotelCount != result.First().Occurences){
                _popularHotel = result.First().NameFiled;
                _popularHotelCount = result.First().Occurences;
                Console.Out.WriteLine($"New Popular Hotel: {_popularHotel}");
                isChanged = true;
            }
            
            //Transport
            result = _preferencessSet.GroupBy(item => item.TransportType).Select(grp =>
                new {
                    NameFiled = grp.Key, 
                    Occurences = grp.Count()
                }).OrderByDescending(x => x.Occurences).ToList();

            findResult = result.Find(x => x.NameFiled == _popularTransport);
            
            if (findResult== null || _popularTransportCount != result.First().Occurences){
                _popularTransport = result.First().NameFiled;
                _popularTransportCount = result.First().Occurences;
                Console.Out.WriteLine($"New Popular Transport: {_popularTransport}");
                isChanged = true;
            }
            
            //RoomType
            result = _preferencessSet.GroupBy(item => item.NameOfRoom).Select(grp =>
                new {
                    NameFiled = grp.Key, 
                    Occurences = grp.Count()
                }).OrderByDescending(x => x.Occurences).ToList();

            findResult = result.Find(x => x.NameFiled == _popularRoom);
            
            if (findResult== null || _popularRoomCount != result.First().Occurences){
                _popularRoom = result.First().NameFiled;
                _popularRoomCount = result.First().Occurences;
                Console.Out.WriteLine($"New Popular RoomType: {_popularRoom}");
                isChanged = true;
            }
            
            if (isChanged){
                //TODO NOTIFY_ABOUT_CHANGE_OF_ROOM_OR_HOTEL_OR_TRANSPORT
            }
        }

        public void GetTopDirectionPerference()
        {
            var result = _eventsSet.GroupBy(item => item.Country).Select(grp =>
                new {
                    Country = grp.Key, 
                    Occurences = grp.Count()
                }).OrderByDescending(x => x.Occurences).ToList();

            var findResult = result.Find(x => x.Country == _popularCountry);
            
            if (findResult== null || _popularCountryCount != result.First().Occurences){
                _popularCountry = result.First().Country;
                _popularCountryCount = result.First().Occurences;
                Console.Out.WriteLine($"New Popular country: {_popularCountry}");
                NotifyAboutNewPopularCountry();
            }

        }
        
        public string GetPerferences()
        {
            throw new NotImplementedException();
            return _popularCountry;
        }

        private void NotifyAboutNewPopularCountry()
        {
            _publishEndpoint.Publish(new NotifyAboutNewPopularCountryQuery
            {
                CountryName = _popularCountry
            });
        }
    }

   
}