using System.Threading.Tasks;
using CommonComponents;
using MassTransit;
using TripService.Services;

namespace TripService.Consumers
{
    public class GetNotificationAboutPopularCountryConsumer : IConsumer<GetNotificationAboutPopularCountryQuery>
    {
        private DepartueDirectionsPerferances _departuePerferancesService;

        public GetNotificationAboutPopularCountryConsumer(DepartueDirectionsPerferances departuePerferancesService)
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