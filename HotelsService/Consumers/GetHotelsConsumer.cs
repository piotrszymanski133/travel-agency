using System;
using System.Threading.Tasks;
using HotelsService.Models;
using HotelsService.Queries;
using HotelsService.Repositories;
using MassTransit;

namespace HotelsService.Consumers
{
    public class GetHotelsConsumer : IConsumer<GetHotelsQuery>
    {
        private IHotelRepository _hotelRepository;

        public GetHotelsConsumer(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }
        public Task Consume(ConsumeContext<GetHotelsQuery> context)
        {
            foreach (HotelWithDescription hotel in _hotelRepository.GetAllHotels())
            {
                Console.WriteLine($"{hotel.Name} {hotel.Description}");
            }
            return Task.CompletedTask;
        }
    }

    public class GetHotelsConsumerDefinition : ConsumerDefinition<GetHotelsConsumer>
    {
        public GetHotelsConsumerDefinition()
        {
            EndpointName = "get-hotels-queue";
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<GetHotelsConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100,200,500,800,1000));
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}