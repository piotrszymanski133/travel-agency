using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Models;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;

namespace TripService.Consumers
{
    public class GetTripsQueryConsumer : IConsumer<GetTripsQuery>
    {
        private IRequestClient<GetHotelsQuery> _hotelclient;
        private IRequestClient<GetTransportQuery> _transportclient;

        public GetTripsQueryConsumer(IRequestClient<GetHotelsQuery> client,
            IRequestClient<GetTransportQuery> transportclient)
        {
            _hotelclient = client;
            _transportclient = transportclient;
        }

        public async Task Consume(ConsumeContext<GetTripsQuery> context)
        {
            TripParameters tripParameters = context.Message.TripParameters;

            List<Trip> trips = new List<Trip>();

            var hotelResponse = await _hotelclient.GetResponse<GetHotelsResponse>(
                new GetHotelsQuery
                {
                    TripParameters = tripParameters
                });

            List<Hotel> hotelsList = hotelResponse.Message.Hotels;

            foreach (var hotel in hotelsList)
            {
                trips.Add(new Trip()
                {
                    Hotel = hotel
                });
            }

            await context.RespondAsync(new GetTripsResponse { Trips = trips, });
        }
    }
}