using System;
using System.Threading.Tasks;
using ApiGateway.Hubs;
using ApiGateway.Hubs.Clients;
using MassTransit;
using CommonComponents;
using Microsoft.AspNetCore.SignalR;

namespace ApiGateway.Consumers
{
    public class ChangeHotelAvailabilityConsumer : IConsumer<ChangeHotelAvailabilityQuery>
    {
        private IHubContext<NotificationHub, INotificationClient> _hub;

        public ChangeHotelAvailabilityConsumer(IHubContext<NotificationHub, INotificationClient> hub)
        {
            _hub = hub;
        }

        public Task Consume(ConsumeContext<ChangeHotelAvailabilityQuery> context)
        {
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