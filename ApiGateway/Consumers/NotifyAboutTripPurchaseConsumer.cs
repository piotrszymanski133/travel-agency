using System.Threading.Tasks;
using ApiGateway.Hubs;
using ApiGateway.Hubs.Clients;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using CommonComponents;

namespace ApiGateway.Consumers
{
    public class NotifyAboutTripPurchaseConsumer : IConsumer<NotifyAboutTripPurchaseQuery>
    {
        private IHubContext<NotificationHub, INotificationClient> _hub;

        public NotifyAboutTripPurchaseConsumer(IHubContext<NotificationHub, INotificationClient> hub)
        {
            _hub = hub;
        }
        
        public async Task Consume(ConsumeContext<NotifyAboutTripPurchaseQuery> context)
        {
            await _hub.Clients.All.SendMessage(new PurchaseNotification
            {
                Message = $"Użytkownik {context.Message.UserName} zarezerwował właśnie pokój w tym hotelu!",
                HotelId = context.Message.HotelId
            });
        }
    }
}