using System.Threading.Tasks;
using CommonComponents;
using HotelsService.Services;
using MassTransit;

namespace HotelsService.Consumers
{
    public class GetDestinationsConsumer: IConsumer<GetDestinationsQuery>
    {
        private IHotelService _hotelService;

        public GetDestinationsConsumer(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }
        public async Task Consume(ConsumeContext<GetDestinationsQuery> context)
        {
            await context.RespondAsync(new GetDestinationsResponse()
            {
                Destinations = _hotelService.GetAllDestinations()
            });
        }
    }
}