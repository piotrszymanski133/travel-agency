using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransportService.Models;

namespace TransportService.Consumer
{
    public class GetTransportConsumer: IConsumer<GetTransportQuery>
    {
        private readonly ILogger<GetTransportQuery> _logger;

        public GetTransportConsumer(ILogger<GetTransportQuery> logger)
        {

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<GetTransportQuery> context)
        {
            var command = context.Message;
            var city = command.DestinationCity;
            using var db = new transportsdbContext();

            //db.Hotels.Include(h => h.Destination).Include(h => h.Hotelrooms).ThenInclude(r => r.Roomtype).ToList();
            var transports = db.Transports.Include(h =>h.DestinationPlaces).Include(h => h.SourcePlaces).ToList();

            var first = transports.First();


            await context.RespondAsync<GetTransportResponse>( new GetTransportResponse(){
                Transports = new List<CommonComponents.Models.Transport>()
                {
                    new CommonComponents.Models.Transport()
                    {
                        Id = "1",
                        DestinationCity = first.DestinationPlaces.City,
                        DestinationCountry = first.DestinationPlaces.Country,
                        Name = first.Transporttype
                    }

                }
            });
        }
    }
}