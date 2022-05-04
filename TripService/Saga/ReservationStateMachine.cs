using System;
using MassTransit;
using CommonComponents;
using MassTransit.Contracts.JobService;

namespace TripService.Saga
{
    public class ReservationStateMachine: MassTransitStateMachine<ReservationState>
    {
        public State ReservationActive { get; private set; }
        public State ReservationOrderd { get; private set; }
        public ReservationStateMachine()
        {
            InstanceState(x => x.CurrentState);
            Event(() => ReserveTrip, x
                => x.CorrelateById(state => state.CorrelationId,context=>context.Message.ReservationId)
                    .SelectId(ctx => ctx.Message.ReservationId));
            Initially(
                    When(ReserveTrip)
                        .ThenAsync(ctx =>
                        {
                            return Console.Out.WriteLineAsync(
                                $"Klient rozpoczal zamowienie {ctx.Message.ReserveTripOfferParameters.HotelId}");
                        })
                        .TransitionTo(ReservationActive)
                    );
            During(ReservationActive,When(ReserveTrip).ThenAsync(ctx => 
                {
                            return Console.Out.WriteLineAsync(
                                $"Klient zakonczyl zamowienie {ctx.Message.ReserveTripOfferParameters.HotelId}"); 
                }).Finalize());
            SetCompletedWhenFinalized();
        }
        
        public Event<ReserveTripQuery> ReserveTrip { get; private set; }
    }
}