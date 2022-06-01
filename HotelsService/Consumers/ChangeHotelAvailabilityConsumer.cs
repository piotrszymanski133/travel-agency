using System;
using System.Threading.Tasks;
using HotelsService.Repositories;
using MassTransit;
using CommonComponents;

namespace HotelsService.Consumers
{
    public class ChangeHotelAvailabilityConsumer : IConsumer<ChangeHotelAvailabilityQuery>
    {
        private IHotelRepository _hotelRepository;


        public ChangeHotelAvailabilityConsumer(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public Task Consume(ConsumeContext<ChangeHotelAvailabilityQuery> context)
        {
            _hotelRepository.ChangeHotelAvaliabilities(context.Message.IdentifierList, context.Message.ChangeQuantity);
            return Task.CompletedTask;
        }
    }
}