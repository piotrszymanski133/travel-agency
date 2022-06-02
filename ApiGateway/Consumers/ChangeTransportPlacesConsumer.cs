using System;
using System.Threading.Tasks;
using ApiGateway.Hubs;
using ApiGateway.Hubs.Clients;
using ApiGateway.Services;
using MassTransit;
using CommonComponents;
using Microsoft.AspNetCore.SignalR;

namespace ApiGateway.Consumers
{
    public class ChangeTransportPlacesConsumer : IConsumer<ChangeTransportPlacesQuery>
    {
        private IHubContext<NotificationHub, INotificationClient> _hub;
        private ILastChangesService _lastChangesService;

        public ChangeTransportPlacesConsumer(IHubContext<NotificationHub, INotificationClient> hub, ILastChangesService lastChangesService)
        {
            _hub = hub;
            _lastChangesService = lastChangesService;
        }

        public Task Consume(ConsumeContext<ChangeTransportPlacesQuery> context)
        {
            var msg = context.Message;
            _lastChangesService.AddTransportChangeEvent(msg);
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