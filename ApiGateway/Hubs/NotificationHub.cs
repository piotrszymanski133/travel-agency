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
        private IRequestClient<GetNotificationAboutPopularTripConfigurationQuery> _notificationTripClient;

        public NotificationHub(IRequestClient<GetNotificationAboutPopularCountryQuery> notificationCountryClient,
            IRequestClient<GetNotificationAboutPopularTripConfigurationQuery> notificationTripClient)
        {
            _notificationCountryClient = notificationCountryClient;
            _notificationTripClient = notificationTripClient;
        }

        public async Task GetPopularCountry()
        {
            var response = _notificationCountryClient.GetResponse<GetNotificationAboutPopularCountryResponse>
                ( new GetNotificationAboutPopularCountryQuery());
            
            if (response.Result.Message.CountryName == string.Empty)
            {
                return;
            }
            
            await Clients.Caller.SendPopularCountryMessage(new PopularCountryNotification
            {
                Country = $"Aktualnie najczęściej wybierany kraj wycieczek to {response.Result.Message.CountryName}"
            });
        }

        public async Task GetPopularHotelConfiguration()
        {
            var response = _notificationTripClient.GetResponse<GetNotificationAboutPopularTripConfigurationResponse>
                (new GetNotificationAboutPopularTripConfigurationQuery());
            
            if (response.Result.Message.Hotel == string.Empty)
            {
                return;
            }
            
            await Clients.Caller.SendPopularTripConfigurationMessage(new PopularTripConfigurationNotification
            {
                Message = $"Aktualnie najczęściej wybierana wycieczka to Hotel: {response.Result.Message.Hotel}, pokój:" +
                          $"{response.Result.Message.Room}, transport: {response.Result.Message.Transport}"
            });
        }
    }
}