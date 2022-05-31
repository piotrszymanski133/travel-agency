using System.Threading.Tasks;
using ApiGateway.Hubs.Clients;
using CommonComponents;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace ApiGateway.Hubs
{
    public class NotificationHub : Hub<INotificationClient>
    {
        private IRequestClient<GetNotificationAboutPopularCountryQuery> _notificationCountryClient;

        public NotificationHub(IRequestClient<GetNotificationAboutPopularCountryQuery> notificationCountryClient)
        {
            _notificationCountryClient = notificationCountryClient;
        }

        public async Task GetPopularCountry()
        {
            var response = _notificationCountryClient.GetResponse<GetNotificationAboutPopularCountryResponse>
                ( new GetNotificationAboutPopularCountryQuery());
            
            if (response.Result.Message.CountryName == string.Empty)
            {
                return;
            }
            
            await Clients.Caller.SendPopularCountryMessage(new PopularCountryNotificationMessage
            {
                Country = $"Aktualnie najczęściej wybierany kraj wycieczek to {response.Result.Message.CountryName}"
            });
        }
    }
}