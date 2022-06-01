using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Models;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;
using TripService.Models;
using TripService.Repository;
using TripService.Services;

namespace TripService.Consumers
{
    public class NewReservationCompleatedQueryConsumer : IConsumer<NewReservationCompleatedQuery>
    {

        private ITripsRepository _tripsRepository;
        private IDepartureDirectionsPreferences _departureDirectionsPreferences;

        public NewReservationCompleatedQueryConsumer(ITripsRepository tripsRepository, IDepartureDirectionsPreferences departureDirectionsPreferences)
        {
            _tripsRepository = tripsRepository;
            _departureDirectionsPreferences = departureDirectionsPreferences;
        }

  

        public async Task Consume(ConsumeContext<NewReservationCompleatedQuery> context)
        {
            var msg = context.Message;
            
            _departureDirectionsPreferences.AddDirectionEvent(new PurchaseDirectionEvents()
            {
                Country = msg.CountryName,
                EventDate = msg.EventDate
            });
            _departureDirectionsPreferences.AddPreferencesEvent(new PurchasePreferencesEvents
            {
                HotelName = msg.HotelName,
                EventDate = msg.EventDate,
                NameOfRoom = msg.NameOfRoom,
                TransportType = msg.TransportType
            });
            
        }
    }
}