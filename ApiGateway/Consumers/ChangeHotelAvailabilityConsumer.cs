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
    public class ChangeHotelAvailabilityConsumer : IConsumer<ChangeHotelAvailabilityQuery>
    {
        private IHubContext<NotificationHub, INotificationClient> _hub;
        private ILastChangesService _lastChangesService;

        public ChangeHotelAvailabilityConsumer(IHubContext<NotificationHub, INotificationClient> hub, ILastChangesService lastChangesService)
        {
            _hub = hub;
            _lastChangesService = lastChangesService;
        }

        public Task Consume(ConsumeContext<ChangeHotelAvailabilityQuery> context)
        {
            var msg = context.Message;
            _lastChangesService.AddHotelChangeEvent(msg);
            _hub.Clients.All.SendHotelStateChangeMessage(new HotelStateChangeNotification
            {
                StartDate = context.Message.startDate,
                EndDate = context.Message.endDate,
                HotelId = context.Message.HotelId
            });
            return Task.CompletedTask;
        }
    }
}