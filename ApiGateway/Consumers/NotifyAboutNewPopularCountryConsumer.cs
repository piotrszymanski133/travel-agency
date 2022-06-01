using System.Threading.Tasks;
using ApiGateway.Hubs;
using ApiGateway.Hubs.Clients;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using CommonComponents;

namespace ApiGateway.Consumers
{
    public class NotifyAboutNewPopularCountryConsumer : IConsumer<NotifyAboutNewPopularCountryQuery>
    {
        
        private IHubContext<NotificationHub, INotificationClient> _hub;

        public NotifyAboutNewPopularCountryConsumer(IHubContext<NotificationHub, INotificationClient> hub)
        {
            _hub = hub;
        }

        public async Task Consume(ConsumeContext<NotifyAboutNewPopularCountryQuery> context)
        {
            await _hub.Clients.All.SendPopularCountryMessage(new PopularCountryNotification
            {
                Country = $"Aktualnie najczęściej wybierany kraj wycieczek to {context.Message.CountryName}"
            });
        }
    }
}