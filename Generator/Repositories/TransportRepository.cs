using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Generator
{
    public interface ITransportRepository
    {
        public Transport GetRandomTransport();
    }
    public class TransportRepository : ITransportRepository
    {
        public Transport GetRandomTransport()
        {
            using var db = new transportsdbContext();
            List<Transport> transports = db.Transports
                .Include(transport => transport.DestinationPlaces)
                .Include(transport => transport.SourcePlaces)
                .Include(transport => transport.Transportevents)
                .ToList();
            
            Random rnd = new Random();
            return transports[rnd.Next(transports.Count)];
        }
    }
}