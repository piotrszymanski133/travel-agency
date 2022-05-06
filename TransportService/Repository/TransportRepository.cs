using System;
using System.Collections.Generic;
using System.Linq;
using CommonComponents.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        TransportService.Models.Transport GetTransport(long id);
        (string, bool) ReserveTransport(long commandDepartueTransportId, long commandReturnTransportId,
            int commandPlaces, Guid commandReservationId,DateTime StartDate,DateTime EndDate, string username);

        void RollbackReserveTransport(Guid commandReservationId);
        void ConfirmTransport(Guid commandReservationId);
        List<UserTransports> getUserTransportsInfo(string msgUsername);
    }



    public class TransportRepository : ITransportRepository
    {
        public List<Transport> GetGeneralTransport(string Departure, string Destination, DateOnly Starttime, int Places,
            int direction)
        {
            List<Transport> newlist = new();
            using (var context = new transportsdbContext())
            {
                List<TransportService.Models.Transport> transportList;
                if (direction == 0)
                {
                    transportList = context.Transports.AsNoTracking()
                        .Include(s => s.DestinationPlaces)
                        .Include(s => s.SourcePlaces)
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
                        .Include(s => s.SourcePlaces)
                        .Where(s => s.DestinationPlaces.City == Destination)
                        .Where(s => s.DestinationPlaces.Country == "Polska")
                        .Where(s => s.SourcePlaces.Country == Departure)
                        .Where(s => s.Transportdate == Starttime)
                        .Where(s => s.Places >= Places).ToList();
                }

                foreach (var transport in transportList)
                {
                    var eventReserved = context.Transportevents.AsNoTracking().Where(s => s.TransportId == transport.Id)
                        .Sum(s => s.Places);

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

        public List<Transport> GetSpecificTransport(string DepartureCity, string DepartueCountry,
            string DestinationCity,
            string DestinationCountry, DateOnly Starttime, int Places)
        {
            List<Transport> newlist = new();
            using (var context = new transportsdbContext())
            {
                var transportList = context.Transports.AsNoTracking()
                    .Where(s => s.Transportdate == Starttime)
                    .Where(s => s.Places >= Places)
                    .Where(s => s.DestinationPlaces.Country == DestinationCountry)
                    .Where(s => s.DestinationPlaces.City == DestinationCity)
                    .Where(s => s.SourcePlaces.City == DepartureCity)
                    .Where(s => s.SourcePlaces.Country == DepartueCountry)
                    .Include(s => s.DestinationPlaces)
                    .Include(s => s.SourcePlaces).ToList();

                foreach (var transport in transportList)
                {
                    var eventReserved = context.Transportevents.AsNoTracking().Where(s => s.TransportId == transport.Id)
                        .Sum(s => s.Places);

                    if (transport.Places - eventReserved >= Places)
                    {
                        newlist.Add(new Transport()
                        {
                            DepartueCity = transport.SourcePlaces.City,
                            DepartueCountry = transport.SourcePlaces.Country,
                            DestinationCity = transport.DestinationPlaces.City,
                            DestinationCountry = transport.DestinationPlaces.Country,
                            Name = transport.Transporttype,
                            Id = transport.Id,
                            Quantity = transport.Places - eventReserved
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
                var x = toMatches.Any(s => s.Name == transport.Name &&
                                           s.DepartueCountry == transport.DestinationCountry &&
                                           s.DepartueCity == transport.DestinationCity &&
                                           s.DestinationCity == transport.DepartueCity &&
                                           s.DestinationCountry == transport.DepartueCountry);

                if (x)
                {
                    finalList.Add(transport);
                }
            }

            return finalList;
        }

        public List<TransportOffer> MatchSpecyficTransports(List<Transport> fromMatches, List<Transport> toMatches,
            int Persons)
        {
            List<TransportOffer> finalList = new();

            foreach (var transport in fromMatches)
            {
                var xx = toMatches.Find(s => s.Name == transport.Name
                                             && s.DepartueCountry == transport.DestinationCountry
                                             && s.DepartueCity == transport.DestinationCity
                                             && s.DestinationCity == transport.DepartueCity
                                             && s.DestinationCountry == transport.DepartueCountry);

                if (xx != null)
                {
                    finalList.Add(new TransportOffer()
                    {
                        TransportIDFrom = transport.Id,
                        DepartureCity = transport.DepartueCity,
                        DepartureCountry = transport.DepartueCountry,
                        DestinationCity = transport.DestinationCity,
                        DestinationCountry = transport.DestinationCountry,
                        Persons = Persons,
                        TransportIDTo = xx.Id,
                        TransportName = transport.Name,
                        Quantity = Math.Min(transport.Quantity,xx.Quantity)
                    });
                }
            }

            return finalList;
        }

        public TransportService.Models.Transport GetTransport(long id)
        {
            using var db = new transportsdbContext();
            return db.Transports
                .Include(transport => transport.DestinationPlaces)
                .First(transport => transport.Id == id);
        }

        public (string, bool) ReserveTransport(long commandDepartueTransportId, long commandReturnTransportId, int commandPlaces,
            Guid commandReservationId, DateTime StartDate, DateTime EndDate,string username)
        {
            var gui1 = Guid.NewGuid();
            var gui2 = Guid.NewGuid();

            string transportTypeName = null;
            using (var context = new transportsdbContext())
            {
                //Some Asserts
                var transportDepartue = context.Transports.Where(x => x.Id == commandDepartueTransportId).AsNoTracking()
                    .ToList();
                var transportReturn = context.Transports.Where(x => x.Id == commandReturnTransportId).AsNoTracking()
                    .ToList();
                
               
                
                var bookedtransport1 = context.Transportevents
                    .Where(x => x.TransportId == commandDepartueTransportId)
                    .Sum(x => x.Places);

                var bookedtransport2 = context.Transportevents
                    .Where(x => x.TransportId == commandReturnTransportId)
                    .Sum(x => x.Places);

                if (transportDepartue.Count != 1 || transportReturn.Count != 1 ||
                    transportDepartue[0].Places - bookedtransport1 < commandPlaces ||
                    transportReturn[0].Places - bookedtransport2 < commandPlaces ||
                    transportDepartue[0].Transportdate !=
                    new DateOnly(StartDate.Year, StartDate.Month, StartDate.Day) ||
                    transportReturn[0].Transportdate != new DateOnly(EndDate.Year, EndDate.Month, EndDate.Day))
                {
                    return (transportReturn[0].Transporttype, false);
                }

                // Console.Out.WriteLine($"R1: {gui1}, R2: {gui2}");
                var reservation_departue = new Transportevent()
                {
                    TransportId = commandDepartueTransportId,
                    Places = commandPlaces,
                    Type = "Reservation",
                    Id =gui1,
                    EventID = commandReservationId,
                    Username = username
                    
                };

                var reservation_return = new Transportevent()
                {
                    TransportId = commandReturnTransportId,
                    Places = commandPlaces,
                    Type = "Reservation",
                    Id = gui2,
                    EventID = commandReservationId,
                    Username = username
                };

                transportTypeName = transportDepartue[0].Transporttype;
                context.Transportevents.Add(reservation_departue);
                context.Transportevents.Add(reservation_return);
                context.SaveChanges();
                
            }
            return (transportTypeName, true);
        }

        public void RollbackReserveTransport(Guid commandReservationId)
        {
            using (var context = new transportsdbContext())
            {
                var newResult = context.Transportevents.Where(x => x.EventID == commandReservationId).ToList();
                
                foreach (var var in newResult)
                {
                    context.Remove(var);
                }
                context.SaveChanges();
            }
        }

        public void ConfirmTransport(Guid commandReservationId)
        {
            using (var context = new transportsdbContext())
            {
                var result = context.Transportevents.Where(e => e.EventID == commandReservationId).ToList();

                foreach (var transportEvent in result)
                {
                    transportEvent.Type = "Ordered";
                }
                context.SaveChanges();
            }
            
        }

        public List<UserTransports> getUserTransportsInfo(string msgUsername)
        {
            HashSet<UserTransports> xx = new();
            using (var context = new transportsdbContext())
            {
                var result = context.Transportevents.Where(key => key.Username==msgUsername).ToList();

                foreach (var transportevent in result)
                {
                    xx.Add(new UserTransports()
                    {
                        Persons = transportevent.Places,
                        TransportTypeName = transportevent.Type,
                        EventID = transportevent.EventID
                    });
                }
            }

            List <UserTransports>  returnList = xx.ToList();

            return returnList;
        }
    }
}