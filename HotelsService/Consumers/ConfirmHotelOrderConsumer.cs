using System;
using System.Threading.Tasks;
using HotelsService.Services;
using CommonComponents;
using MassTransit;

namespace HotelsService.Consumers
{
    public class ConfirmHotelOrderConsumer : IConsumer<ConfirmHotelOrderQuery>
    {
        private IHotelService _hotelService;

        public ConfirmHotelOrderConsumer(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }
        public Task Consume(ConsumeContext<ConfirmHotelOrderQuery> context)
        {
            _hotelService.confirmOrder(context.Message.ReservationId);
            Console.WriteLine($"Hotel purchase confirmed for id {context.Message.ReservationId}");

            return Task.CompletedTask;
        }
    }
}