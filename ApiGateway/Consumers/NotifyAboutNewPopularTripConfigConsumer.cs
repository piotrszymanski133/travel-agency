using System.Threading.Tasks;
using ApiGateway.Hubs;
using ApiGateway.Hubs.Clients;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using CommonComponents;

namespace ApiGateway.Consumers
{
    public class NotifyAboutNewPopularTripConfigConsumer: IConsumer<NotifyAboutNewPopularTripConfigQuery>
    {
                
        private IHubContext<NotificationHub, INotificationClient> _hub;

        public NotifyAboutNewPopularTripConfigConsumer(IHubContext<NotificationHub, INotificationClient> hub)
        {
            _hub = hub;
        }
        
        public async Task Consume(ConsumeContext<NotifyAboutNewPopularTripConfigQuery> context)
        {
            await _hub.Clients.All.SendPopularTripConfigurationMessage(new PopularTripConfigurationNotification
            {
                Message = $"Aktualnie najczęściej wybierana wycieczka to Hotel: {context.Message.Hotel}, pokój:" +
                          $"{context.Message.Room}, transport: {context.Message.Transport}"
            });
        }
    }
}