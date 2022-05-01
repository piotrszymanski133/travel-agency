using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var hotelresponse = await _hotelclient.GetResponse<GetHotelsRespond>(new GetHotelsQuery());
            List<Trip> trips = new List<Trip>();
            foreach (Hotel hotel in hotelresponse.Message.Hotels)
            {
                trips.Add(new Trip {Hotel = hotel});
            }
            
            var transportresponse =  await _transportclient.GetResponse<GetTransportRespond>(new GetTransportQuery()
            {
                DestinationCity = "City"
            });

            for (var i = 0; i < trips.Count; i++)
            {
                trips[i].Transport = transportresponse.Message.Transports.First();
            }
            
            await context.RespondAsync(new GetTripsRespond {Trips = trips,});
        }
    }

}
