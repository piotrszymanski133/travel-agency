using System.Collections.Generic;
using System.Threading.Tasks;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;

namespace TripService.Consumers
{
    public class GetTripOfferQueryConsumer : IConsumer<GetTripOfferQuery>
    {
        private IRequestClient<GetHotelOfferQuery> _hotelclient;
        private IRequestClient<GetTransportOffersQuery> _transportclient;
        
        public GetTripOfferQueryConsumer(IRequestClient<GetHotelOfferQuery> client, IRequestClient<GetTransportOffersQuery> transportclient)
        {
            _hotelclient = client;
            _transportclient = transportclient;
        }
        
        
        public async Task Consume(ConsumeContext<GetTripOfferQuery> context)
        {
            TripOfferQueryParameters tripOfferQueryParameters = context.Message.TripOfferQueryParameters;
            
            var hotelResponse = await _hotelclient.GetResponse<GetHotelOfferResponse>(
                new GetHotelOfferQuery()
                {
                    TripOfferQueryParameters = tripOfferQueryParameters
                });
            
            var transportResponse =  await _transportclient.GetResponse<GetTransportOffersResponse>(
                new GetTransportOffersQuery()
                {
                    DepartueCountry = "Polska",
                    DepartueCity = tripOfferQueryParameters.Departure,
                    DepartureDate = tripOfferQueryParameters.StartDate,
                    DestinationCity = hotelResponse.Message.HotelOffer.DestinationCity,
                    DestinationCountry = hotelResponse.Message.HotelOffer.DestinationCountry,
                    Places = tripOfferQueryParameters.Adults + tripOfferQueryParameters.ChildrenUnder3 + 
                             tripOfferQueryParameters.ChildrenUnder10+tripOfferQueryParameters.ChildrenUnder18,
                    ReturnDate = tripOfferQueryParameters.EndDate,
                });
            TripOffer tripOffer = new TripOffer
            {
                StartDate = tripOfferQueryParameters.StartDate,
                EndDate = tripOfferQueryParameters.EndDate,
                HotelOffer = hotelResponse.Message.HotelOffer,
                TransportOffers = transportResponse.Message.TransportOffer
            };
            await context.RespondAsync(new GetTripOfferResponse
            {
                TripOffer = tripOffer
            });
        }
    }
}