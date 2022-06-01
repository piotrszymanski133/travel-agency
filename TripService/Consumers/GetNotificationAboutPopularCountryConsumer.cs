using System.Threading.Tasks;
using CommonComponents;
using MassTransit;
using TripService.Services;

namespace TripService.Consumers
{
    public class GetNotificationAboutPopularCountryConsumer : IConsumer<GetNotificationAboutPopularCountryQuery>
    {
        private IDepartureDirectionsPreferences _departurePreferencesService;

        public GetNotificationAboutPopularCountryConsumer(IDepartureDirectionsPreferences departurePreferencesService)
        {
            _departurePreferencesService = departurePreferencesService;
        }

        public async Task Consume(ConsumeContext<GetNotificationAboutPopularCountryQuery> context)
        {
            await context.RespondAsync(new GetNotificationAboutPopularCountryResponse()
            {
                CountryName = _departurePreferencesService.GetCountryPreferences()
            });
        }
    }
}