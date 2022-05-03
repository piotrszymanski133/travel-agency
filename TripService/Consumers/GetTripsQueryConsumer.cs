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
            
            var transportResponse =  await _transportclient.GetResponse<GetTransportResponse>(new GetTransportQuery()
            {
                Destination = context.Message.TripParameters.Destination,
                Departue = context.Message.TripParameters.Departure,
                DepartureDate = context.Message.TripParameters.StartDate,
                ReturnDate = context.Message.TripParameters.EndDate,
                Places = context.Message.TripParameters.Adults + context.Message.TripParameters.ChildrenUnder3 + 
                         context.Message.TripParameters.ChildrenUnder10+context.Message.TripParameters.ChildrenUnder18

            });

            List<Hotel> hotelsList = hotelResponse.Message.Hotels;
            List<Transport> transportsList = transportResponse.Message.Transports;

            foreach (var hotel in hotelsList)
            {
                if (transportsList.Any(x =>
                    x.DestinationCountry == hotel.DestinationCountry && x.DestinationCity == hotel.DestinationCity))
                {
                    trips.Add(new Trip()
                    {
                        Hotel = hotel
                    });
                }
                
            }
            await context.RespondAsync(new GetTripsResponse {Trips = trips,});
        }
    }

}
