using System.Collections.Generic;
using System.Threading.Tasks;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;

namespace TripService.Consumers
{
    public class GetTripsQueryConsumer : IConsumer<GetTripsQuery>
    {
        private IRequestClient<GetHotelsQuery> _client;
        //TODO: ADD TRANSPORT
        public GetTripsQueryConsumer(IRequestClient<GetHotelsQuery> client)
        {
            _client = client;
        }

        public async Task Consume(ConsumeContext<GetTripsQuery> context)
        {
            var response = await _client.GetResponse<GetHotelsRespond>(new GetHotelsQuery());
            List<Trip> trips = new List<Trip>();
            foreach (Hotel hotel in response.Message.Hotels)
            {
                trips.Add(new Trip {Hotel = hotel});
            }

            await context.RespondAsync(new GetTripsRespond {Trips = trips});
        }
    }

}
