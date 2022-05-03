using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Models;
using CommonComponents;
using CommonComponents.Models;
using HotelsService.Repositories;
using MassTransit;
using HotelsService.Models;
using Hotel = CommonComponents.Models.Hotel;

namespace HotelsService.Consumers
{
    public class GetHotelsConsumer : IConsumer<GetHotelsQuery>
    {
        private IHotelRepository _hotelRepository;

        public GetHotelsConsumer(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }
        public async Task Consume(ConsumeContext<GetHotelsQuery> context)
        {
            TripParameters tripParameters = context.Message.TripParameters;
            
            List<Hotel> matchedHotels = _hotelRepository.GetHotels(tripParameters);
            //_hotelRepository.CreateReservationEvent(tripParameters.StartDate, tripParameters.EndDate);

            await context.RespondAsync(new GetHotelsResponse { Hotels = matchedHotels});
        }
    }
}