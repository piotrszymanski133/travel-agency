using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MassTransit.Configuration;

namespace TripService.Services
{
    public class CustomComparator : IComparer<PurchaseDirectionEvents>
    {
        public int Compare(PurchaseDirectionEvents x, PurchaseDirectionEvents y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return x.EventDate.CompareTo(y.EventDate);
        }
    }

    public interface IDepartueDirectionsPerferances
    {
        public void AddEvent(PurchaseDirectionEvents p_event);
        public void GetTopPerference();
        public string GetPerferences();
    }

    public class DepartueDirectionsPerferances: IDepartueDirectionsPerferances
    {
        private SortedSet<PurchaseDirectionEvents> _eventsSet;
        private string _popularCountry = string.Empty;
        private int _popularCount = 0;

        public DepartueDirectionsPerferances()
        {
            _eventsSet = new SortedSet<PurchaseDirectionEvents>(new CustomComparator());
        }

        public void AddEvent(PurchaseDirectionEvents p_event)
        {
            _eventsSet.Add(p_event);

            if (_eventsSet.Count >= 10)
            {
                _eventsSet.Remove(_eventsSet.Last());
            }

            GetTopPerference();

        }
        public void GetTopPerference()
        {
            var result = _eventsSet.GroupBy(item => item.Country).Select(grp =>
                new {
                    Country = grp.Key, 
                    Occurences = grp.Count()
                }).OrderByDescending(x => x.Occurences).ToList();

            var findResult = result.Find(x => x.Country == _popularCountry);
            
            if (findResult== null || _popularCount != result.First().Occurences){
                _popularCountry = result.First().Country;
                _popularCount = result.First().Occurences;
                Console.Out.WriteLine($"New Popular country: {_popularCountry}");
                //TODO SEND MESSEGE NewPopularCountryQuery
            }

        }
        
        public string GetPerferences()
        {
            return _popularCountry;
        }
    }

   
}