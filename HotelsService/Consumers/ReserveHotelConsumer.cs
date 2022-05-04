using System;
using System.Threading.Tasks;
using CommonComponents;
using HotelsService.Repositories;
using HotelsService.Services;
using MassTransit;

namespace HotelsService.Consumers
{
    public class ReserveHotelConsumer : IConsumer<ReserveHotelQuery>
    {
        private IHotelService _hotelService;

        public ReserveHotelConsumer(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }
        public async Task Consume(ConsumeContext<ReserveHotelQuery> context)
        {
            bool success = _hotelService.tryToReserveHotel(context.Message.ReserveTripOfferParameters, context.Message.ReservationId);
            if (success)
            {
                await context.Publish(new ReserveHotelSuccessResponse() 
                {
                    ReservationId = context.Message.ReservationId,
                });
            }
            else
            {
                await context.Publish(new ReserveHotelFailureResponse() 
                {
                    ReservationId = context.Message.ReservationId,
                });
            }

        }
    }
}