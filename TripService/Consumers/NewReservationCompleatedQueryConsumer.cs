using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Models;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;
using TripService.Repository;
using TripService.Services;

namespace TripService.Consumers
{
    public class NewReservationCompleatedQueryConsumer : IConsumer<NewReservationCompleatedQuery>
    {

        private ITripsRepository _tripsRepository;
        private IDepartueDirectionsPerferances _departueDirectionsPerferances;

        public NewReservationCompleatedQueryConsumer(ITripsRepository tripsRepository, IDepartueDirectionsPerferances departueDirectionsPerferances)
        {
            _tripsRepository = tripsRepository;
            _departueDirectionsPerferances = departueDirectionsPerferances;
        }

  

        public async Task Consume(ConsumeContext<NewReservationCompleatedQuery> context)
        {
            var msg = context.Message;
            
            _departueDirectionsPerferances.AddEvent(new PurchaseDirectionEvents()
            {
                Country = msg.CountryName,
                EventDate = msg.EventDate
            });
            
        }
    }
}