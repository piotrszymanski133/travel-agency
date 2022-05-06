using System.Threading.Tasks;
using CommonComponents;
using MassTransit;
using TripService.Repository;

namespace TripService.Consumers
{
    public class CreateUserTripQueryConsumer: IConsumer<CreateUserTripQuery>
    {
        private ITripsRepository _tripsRepository;

        public CreateUserTripQueryConsumer(ITripsRepository tripsRepository)
        {
            _tripsRepository = tripsRepository;
        }

        public async Task Consume(ConsumeContext<CreateUserTripQuery> context)
        {
            var msg = context.Message;
            _tripsRepository.SetUserTrip(msg);
        }
    }
}