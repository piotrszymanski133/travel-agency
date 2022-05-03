using System;
using MassTransit;
using CommonComponents;
using MassTransit.Contracts.JobService;

namespace TripService.Saga
{
    public class ReservationStateMachine: MassTransitStateMachine<ReservationState>
    {
        public ReservationStateMachine()
        {
            InstanceState(x => x.CurrentState);
            Event(() => ReserveTrip, x
                => x.CorrelateById(ctx => ctx.Message.ReservationId)
                    .SelectId(ctx => Guid.NewGuid()));
            Initially(
                When(ReserveTrip)
                    .ThenAsync(ctx =>
                    {
                        return Console.Out.WriteLineAsync($"Klient rozpoczal zamowienie {ctx.Message.ReserveTripOfferParameters.HotelId}");
                    })
                    .Finalize());
            SetCompletedWhenFinalized();
        }
        
        public Event<ReserveTripQuery> ReserveTrip { get; private set; }
    }
}