using System;
using System.Threading.Tasks;
using ApiGateway.Hubs;
using ApiGateway.Hubs.Clients;
using MassTransit;
using CommonComponents;
using Microsoft.AspNetCore.SignalR;

namespace ApiGateway.Consumers
{
    public class ChangeTransportPlacesConsumer : IConsumer<ChangeTransportPlacesQuery>
    {
        private IHubContext<NotificationHub, INotificationClient> _hub;

        public ChangeTransportPlacesConsumer(IHubContext<NotificationHub, INotificationClient> hub)
        {
            _hub = hub;
        }

        public Task Consume(ConsumeContext<ChangeTransportPlacesQuery> context)
        {
            _hub.Clients.All.SendTransportStateChangeMessage(new TransportStateChangeNotification
            {
                SourcePlacesId = context.Message.SourcePlacesId,
                DestinationPlacesId = context.Message.DestinationPlacesId,
                TransportId = context.Message.TransportId
            });
            return Task.CompletedTask;
        }
    }
}