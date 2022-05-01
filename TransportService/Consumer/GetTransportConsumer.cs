using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

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

            await context.RespondAsync<GetTransportRespond>( new List<Transport>(){});
        }
    }
}