using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransportService.Models;
using TripService.Repository;
using Transport = CommonComponents.Models.Transport;

namespace TransportService.Consumer
{
    public class GetUserTripsTransportConsumer: IConsumer<GetUserTripsTransportQuery>
    {
        private ITransportRepository _repository;

        public GetUserTripsTransportConsumer( ITransportRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<GetUserTripsTransportQuery> context)
        {
            var msg = context.Message;

            var result = _repository.getUserTransportsInfo(msg.Username);
            
            
           
           await context.Publish(new GetUserTripsTransporResponse()
           {
               UserTransportsList = result
           });
        }
    }
}