using System.Threading.Tasks;
using ApiGateway.Models;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;

namespace TripService.Consumers
{
    public class GetUserTripsQueryConsumer : IConsumer<GetUserTripsQuery>
    {
        private IRequestClient<GetHotelOfferQuery> _hotelclient;
        private IRequestClient<GetTransportOffersQuery> _transportclient;

        public GetUserTripsQueryConsumer(IRequestClient<GetHotelOfferQuery> hotelclient, IRequestClient<GetTransportOffersQuery> transportclient)
        {
            _hotelclient = hotelclient;
            _transportclient = transportclient;
        }

        public async Task Consume(ConsumeContext<GetUserTripsQuery> context)
        {
            var msg = context.Message;
          
           //TODO Call Hotel Service for --RESERVATIONS-- for a specific msg.Username
           
           //TODO Call for Transport Service
           
           //TODO Merge list of offers
           
           //TODO Return Offers
           await context.RespondAsync(new GetUserTripsResponse
            {
                
            });
        }
    }
}