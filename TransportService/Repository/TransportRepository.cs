using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TransportService.Models;
using Transport = CommonComponents.Models.Transport;

namespace TripService.Repository
{
    public interface ITransportRepository
    {
        List<Transport> GetGeneralTransport(string Departure,string Destination, DateOnly Starttime,int Places, int direction); 
        List<Transport> MatchTransports(List<Transport> fromMatches, List<Transport> toMatches);
    }

 

    public class TransportRepository : ITransportRepository
    {
        public List<Transport> GetGeneralTransport(string Departure, string Destination, DateOnly Starttime, int Places,int direction)
        {
            List<Transport> newlist = new();
            using(var context = new transportsdbContext())
            {
                List<TransportService.Models.Transport> transportList;
                if (direction == 0)
                {
                     transportList = context.Transports.AsNoTracking()
                        .Include(s => s.DestinationPlaces)
                        .Include(s=>s.SourcePlaces)
                        .Where(s => s.DestinationPlaces.Country == Destination)
                        .Where(s => s.SourcePlaces.City == Departure)
                        .Where(s => s.SourcePlaces.Country == "Polska")
                        .Where(s => s.Transportdate == Starttime)
                        .Where(s => s.Places >= Places).ToList();
                }
                else
                {
                     transportList = context.Transports.AsNoTracking()
                        .Include(s => s.DestinationPlaces)
                        .Include(s=>s.SourcePlaces)
                        .Where(s => s.DestinationPlaces.City == Destination)
                        .Where(s => s.DestinationPlaces.Country == "Polska")
                        .Where(s => s.SourcePlaces.Country == Departure)
                        .Where(s => s.Transportdate == Starttime)
                        .Where(s => s.Places >= Places).ToList();
                }

                
                
       
                
                
                foreach (var transport in transportList)
                {
                    var eventReserved = context.Transportevents.AsNoTracking().Where(s => s.TransportId == transport.Id)
                         .Sum(s=> s.Places);

                      //var sum = eventReserved.Sum(xx => xx.Places);

                      if (transport.Places - eventReserved >= Places)
                    {
                        newlist.Add(new Transport()
                        {
                            DepartueCity = transport.SourcePlaces.City,
                            DepartueCountry = transport.SourcePlaces.Country,
                            DestinationCity = transport.DestinationPlaces.City,
                            DestinationCountry = transport.DestinationPlaces.Country,
                            Name = transport.Transporttype
                        });
                    }
                    
                }
                
            }
            return newlist;
        }

        public List<Transport> MatchTransports(List<Transport> fromMatches, List<Transport> toMatches)
        {
            List<Transport> finalList = new();

            foreach (var transport in fromMatches)
            {
                var x = toMatches.Any(s => s.Name == transport.Name && s.DepartueCountry == transport.DestinationCountry &&
                s.DepartueCity == transport.DestinationCity && s.DestinationCity == transport.DepartueCity && s.DestinationCountry == transport.DepartueCountry);
             
              if (x){
                  finalList.Add(transport);
              }
            }
            return finalList;
        }
    }
}