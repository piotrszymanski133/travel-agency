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
        
        public GetTripsQueryConsumer(IRequestClient<GetHotelsQuery> client, IRequestClient<GetTransportQuery> transportclient)
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
            var transportResponse =  await _transportclient.GetResponse<GetTransportResponse>(
                new GetTransportQuery
                {
                    DestinationCity = "City"
                });

            foreach (Hotel hotel in hotelResponse.Message.Hotels)
            {
                trips.Add(new Trip {Hotel = hotel});
            }
            for (var i = 0; i < trips.Count; i++)
            {
                trips[i].Transport = transportResponse.Message.Transports.First();
            }
            
            await context.RespondAsync(new GetTripsResponse {Trips = trips,});
        }
    }

}
