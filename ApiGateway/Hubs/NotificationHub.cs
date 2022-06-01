using System;
using System.Threading.Tasks;
using ApiGateway.Hubs.Clients;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace ApiGateway.Hubs
{
    public class NotificationHub : Hub<INotificationClient>
    {
        private IRequestClient<GetNotificationAboutPopularCountryQuery> _notificationCountryClient;
        private IRequestClient<GetNotificationAboutPopularTripConfigurationQuery> _notificationTripClient;
        private IRequestClient<GetTripOfferQuery> _tripOfferClient;


        public NotificationHub(IRequestClient<GetNotificationAboutPopularCountryQuery> notificationCountryClient,
            IRequestClient<GetNotificationAboutPopularTripConfigurationQuery> notificationTripClient,
            IRequestClient<GetTripOfferQuery> tripOfferClient)
        {
            _notificationCountryClient = notificationCountryClient;
            _notificationTripClient = notificationTripClient;
            _tripOfferClient = tripOfferClient;
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

        public async Task GetTrip(TripOfferQueryParameters tripOfferQueryParameters)
        {
            if (tripOfferQueryParameters.Adults <= 0 || tripOfferQueryParameters.ChildrenUnder3 < 0 ||
                tripOfferQueryParameters.ChildrenUnder10 < 0 ||
                tripOfferQueryParameters.ChildrenUnder18 < 0 || tripOfferQueryParameters.StartDate < DateTime.Today ||
                tripOfferQueryParameters.EndDate < DateTime.Today ||
                tripOfferQueryParameters.EndDate < tripOfferQueryParameters.StartDate)
            {
                return;
            }
            var response = await _tripOfferClient.GetResponse<GetTripOfferResponse>(new GetTripOfferQuery
            {
                TripOfferQueryParameters = tripOfferQueryParameters
            });
            if (response.Message.TripOffer == null)
            {
                return;
            }
            await Clients.All.SendTripOffer(new UpdatedTrip
            {
                TripOffer = response.Message.TripOffer
            });
        }
    }
}