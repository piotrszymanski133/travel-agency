using System;
using System.Threading.Tasks;
using HotelsService.Queries;
using MassTransit;

namespace HotelsService.Consumers
{
    public class GetHotelsConsumer : IConsumer<GetHotelsQuery>
    {
        public Task Consume(ConsumeContext<GetHotelsQuery> context)
        {
            Console.WriteLine($"{context.Message.City} {context.Message.Country}");
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