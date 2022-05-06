using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Models;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;
using TripService.Repository;

namespace TripService.Consumers
{
    public class GetUserTripsQueryConsumer : IConsumer<GetUserTripsQuery>
    {

        private ITripsRepository _tripsRepository;

        public GetUserTripsQueryConsumer(ITripsRepository tripsRepository)
        {
            _tripsRepository = tripsRepository;
        }

        public async Task Consume(ConsumeContext<GetUserTripsQuery> context)
        { 
            var msg = context.Message;

            List<UserTrips> userTrips = _tripsRepository.GetUserTrips(msg.Username);

            await context.RespondAsync(new GetUserTripsResponse
            {
                userTrips = userTrips
            });
        }
    }
}