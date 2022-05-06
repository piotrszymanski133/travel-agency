using System.Threading.Tasks;
using CommonComponents;
using HotelsService.Services;
using MassTransit;

namespace HotelsService.Consumers
{
    public class GetUserTripsHotelsConsumer : IConsumer<GetUserTripsHotelsQuery>
    {
        private IHotelService _hotelService;

        public GetUserTripsHotelsConsumer(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }
        public async Task Consume(ConsumeContext<GetUserTripsHotelsQuery> context)
        {
            await context.RespondAsync(new GetUserTripsHotelsResponse()
            {
                OrderedHotels = _hotelService.GetUserOrders(context.Message.Username)
            });
        }
    }
}