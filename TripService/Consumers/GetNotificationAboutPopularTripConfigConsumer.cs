using System.Threading.Tasks;
using CommonComponents;
using MassTransit;
using TripService.Models;
using TripService.Services;
using CommonComponents;

namespace TripService.Consumers
{
    public class GetNotificationAboutPopularTripConfigConsumer : IConsumer<GetNotificationAboutPopularTripConfigurationQuery>
    {
        
        private IDepartureDirectionsPreferences _departurePreferencesService;

        public GetNotificationAboutPopularTripConfigConsumer(IDepartureDirectionsPreferences departurePreferencesService)
        {
            _departurePreferencesService = departurePreferencesService;
        }

        public async Task Consume(ConsumeContext<GetNotificationAboutPopularTripConfigurationQuery> context)
        {
            PopularGeneralPreferences preferences = _departurePreferencesService.GetGeneralPreferences();
            await context.RespondAsync(new GetNotificationAboutPopularTripConfigurationResponse
            {
                Hotel = preferences.PopularHotel,
                Room = preferences.PopularRoom,
                Transport = preferences.PopularTransport
            });
        }
    }
}