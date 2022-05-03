using System;
using System.Collections.Generic;
using System.Linq;
using CommonComponents.Models;
using Microsoft.EntityFrameworkCore;
using TransportService.Models;
using Transport = CommonComponents.Models.Transport;

namespace TripService.Repository
{
    public interface ITransportRepository
    {
        List<Transport> GetGeneralTransport(string Departure,string Destination, DateOnly Starttime,int Places, int direction); 
        List<Transport> GetSpecificTransport(string DepartureCity,string DepartueCountry,string DestinationCity,string DestinationCountry, DateOnly Starttime,int Places); 
        List<Transport> MatchTransports(List<Transport> fromMatches, List<Transport> toMatches);
        List<TransportOffer> MatchSpecyficTransports(List<Transport> fromMatches, List<Transport> toMatches,int Persons);
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

        public List<Transport> GetSpecificTransport(string DepartureCity, string DepartueCountry, string DestinationCity,
            string DestinationCountry, DateOnly Starttime, int Places)
        {
           List<Transport> newlist = new();
            using(var context = new transportsdbContext())
            {
                var transportList = context.Transports.AsNoTracking()
                    .Where(s => s.Transportdate == Starttime)
                    .Where(s => s.Places >= Places)
                    .Where(s => s.DestinationPlaces.Country == DestinationCountry)
                    .Where(s => s.DestinationPlaces.City == DestinationCity)
                    .Where(s => s.SourcePlaces.City == DepartureCity)
                    .Where(s => s.SourcePlaces.Country == DepartueCountry)
                    .Include(s => s.DestinationPlaces)
                    .Include(s=>s.SourcePlaces).ToList();
                    
                foreach (var transport in transportList)
                { 
                    var eventReserved = context.Transportevents.AsNoTracking().Where(s => s.TransportId == transport.Id)
                         .Sum(s=> s.Places);
                    
                    if (transport.Places - eventReserved >= Places)
                    {
                        newlist.Add(new Transport()
                        {
                            DepartueCity = transport.SourcePlaces.City,
                            DepartueCountry = transport.SourcePlaces.Country,
                            DestinationCity = transport.DestinationPlaces.City,
                            DestinationCountry = transport.DestinationPlaces.Country,
                            Name = transport.Transporttype,
                            Id = transport.Id
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

        public List<TransportOffer> MatchSpecyficTransports(List<Transport> fromMatches, List<Transport> toMatches,int Persons)
        {
            List<TransportOffer> finalList = new();

            foreach (var transport in fromMatches)
            {
                var xx = toMatches.Find(s => s.Name == transport.Name 
                                             && s.DepartueCountry == transport.DestinationCountry 
                                             &&s.DepartueCity == transport.DestinationCity 
                                             && s.DestinationCity == transport.DepartueCity 
                                             && s.DestinationCountry == transport.DepartueCountry);
                
                if (xx != null){
                    finalList.Add(new TransportOffer()
                    {
                        TransportIDFrom =transport.Id,
                        DepartueCity = transport.DepartueCity,
                        DepartueCountry = transport.DepartueCountry,
                        DestinationCity = transport.DestinationCity,
                        DestinationCountry = transport.DestinationCountry,
                        Persons = Persons,
                        TransportIDTo = xx.Id,
                        TransportName = transport.Name
                    });
                }
            }
            return finalList;
        }
    }
}