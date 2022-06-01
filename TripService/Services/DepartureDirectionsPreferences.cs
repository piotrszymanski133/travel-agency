using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Configuration;
using CommonComponents;
using TripService.Helpers;
using TripService.Models;

namespace TripService.Services
{
    public class DepartureDirectionsPreferences: IDepartureDirectionsPreferences
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

        public DepartureDirectionsPreferences(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            _eventsSet = new SortedSet<PurchaseDirectionEvents>(new CustomDirectionComparator());
            _preferencessSet = new SortedSet<PurchasePreferencesEvents>(new CustomComparator());
        }

        public void AddDirectionEvent(PurchaseDirectionEvents pEvent)
        {
            _eventsSet.Add(pEvent);

            if (_eventsSet.Count >= 10)
            {
                _eventsSet.Remove(_eventsSet.Last());
            }

            GetTopDirectionPreference();

        }

        public void AddPreferencesEvent(PurchasePreferencesEvents pEvent)
        {
            _preferencessSet.Add(pEvent);

            if (_preferencessSet.Count >= 10)
            {
                _preferencessSet.Remove(_preferencessSet.Last());
            }

            GetTopHotelPreference();
        }

        private void GetTopHotelPreference()
        {
            var isChanged = false;
            
            //Hotel
            
            var result = _preferencessSet.GroupBy(item => item.HotelName).Select(grp =>
                new {
                    NameFiled = grp.Key, 
                    Occurences = grp.Count()
                }).OrderByDescending(x => x.Occurences).ToList();

            var findResult = result.Find(x => x.NameFiled == _popularHotel);
            
            if (findResult== null || (_popularHotelCount != result.First().Occurences && _popularHotel!= result.First().NameFiled)){
                _popularHotel = result.First().NameFiled;
                _popularHotelCount = result.First().Occurences;
                Console.Out.WriteLine($"New Popular Hotel: {_popularHotel}");
                isChanged = true;
                //TODO CHANGE HOTEL NAME
            }
            
            //Transport
            result = _preferencessSet.GroupBy(item => item.TransportType).Select(grp =>
                new {
                    NameFiled = grp.Key, 
                    Occurences = grp.Count()
                }).OrderByDescending(x => x.Occurences).ToList();

            findResult = result.Find(x => x.NameFiled == _popularTransport);
            
            if (findResult== null ||( _popularTransportCount != result.First().Occurences && _popularTransport!= result.First().NameFiled)){
                _popularTransport = result.First().NameFiled;
                _popularTransportCount = result.First().Occurences;
                Console.Out.WriteLine($"New Popular Transport: {_popularTransport}");
                isChanged = true;
                //TODO CHANGE TRANSPORT TYPE
            }
            
            //RoomType
            result = _preferencessSet.GroupBy(item => item.NameOfRoom).Select(grp =>
                new {
                    NameFiled = grp.Key, 
                    Occurences = grp.Count()
                }).OrderByDescending(x => x.Occurences).ToList();

            findResult = result.Find(x => x.NameFiled == _popularRoom);
            
            if (findResult== null ||( _popularRoomCount != result.First().Occurences && _popularRoom!=result.First().NameFiled)){
                _popularRoom = result.First().NameFiled;
                _popularRoomCount = result.First().Occurences;
                Console.Out.WriteLine($"New Popular RoomType: {_popularRoom}");
                isChanged = true;
                //TODO CHANGE ROOM TYPE
            }
            
            if (isChanged)
            {
                NotifyAboutNewPopularTripConfiguration();
            }
        }

        public void GetTopDirectionPreference()
        {
            var result = _eventsSet.GroupBy(item => item.Country).Select(grp =>
                new {
                    Country = grp.Key, 
                    Occurences = grp.Count()
                }).OrderByDescending(x => x.Occurences).ToList();

            var findResult = result.Find(x => x.Country == _popularCountry);
            
            if (findResult== null ||( _popularCountryCount != result.First().Occurences && _popularCountry!=result.First().Country)){
                _popularCountry = result.First().Country;
                _popularCountryCount = result.First().Occurences;
                Console.Out.WriteLine($"New Popular country: {_popularCountry}");
                NotifyAboutNewPopularCountry();
            }

        }
        
        public string GetCountryPreferences()
        {
            return _popularCountry;
        }
        public PopularGeneralPreferences GetGeneralPreferences()
        {
            return new PopularGeneralPreferences()
            {
                PopularHotel = _popularHotel,
                PopularTransport = _popularTransport,
                PopularRoom = _popularRoom
                
            };
        }

        private void NotifyAboutNewPopularCountry()
        {
            _publishEndpoint.Publish(new NotifyAboutNewPopularCountryQuery
            {
                CountryName = _popularCountry
            });
        }

        private void NotifyAboutNewPopularTripConfiguration()
        {
            _publishEndpoint.Publish(new NotifyAboutNewPopularTripConfigQuery
            {
                Hotel = _popularHotel,
                Room = _popularRoom,
                Transport = _popularTransport
            });
        }
    }
}