using System;
using System.Threading.Tasks;
using TripService.Repository;
using MassTransit;
using CommonComponents;

namespace TransportService.Consumer
{
    public class ChangeTransportPlacesConsumer : IConsumer<ChangeTransportPlacesQuery>
    {
        private ITransportRepository _transportRepository;


        public ChangeTransportPlacesConsumer(ITransportRepository transportRepository)
        {
            _transportRepository = transportRepository;
        }

        public Task Consume(ConsumeContext<ChangeTransportPlacesQuery> context)
        {
            _transportRepository.ChangeTransportPlaces(context.Message.TransportId, context.Message.ChangePlaces);
            return Task.CompletedTask;
        }
    }
}