using System.Threading.Tasks;
using CommonComponents;
using MassTransit;
using TripService.Services;

namespace TripService.Consumers
{
    public class GetNotificationAboutPopularCountryConsumer : IConsumer<GetNotificationAboutPopularCountryQuery>
    {
        private IDepartueDirectionsPerferances _departuePerferancesService;

        public GetNotificationAboutPopularCountryConsumer(IDepartueDirectionsPerferances departuePerferancesService)
        {
            _departuePerferancesService = departuePerferancesService;
        }

        public async Task Consume(ConsumeContext<GetNotificationAboutPopularCountryQuery> context)
        {
            await context.RespondAsync(new GetNotificationAboutPopularCountryResponse()
            {
                CountryName = _departuePerferancesService.GetPerferences()
            });
        }
    }
}