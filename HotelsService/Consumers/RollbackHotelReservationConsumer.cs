using System.Threading.Tasks;
using HotelsService.Services;
using MassTransit;
using CommonComponents;

namespace HotelsService.Consumers
{
    public class RollbackHotelReservationConsumer : IConsumer<RollbackHotelReservationQuery>
    {
        private IHotelService _hotelService;

        public RollbackHotelReservationConsumer(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }
        
        public Task Consume(ConsumeContext<RollbackHotelReservationQuery> context)
        {
            _hotelService.rollbackReservation(context.Message.TripReservationId);
            return Task.CompletedTask;
        }
    }
}