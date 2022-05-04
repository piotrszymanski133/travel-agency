using System;
using MassTransit;
using CommonComponents;
using MassTransit.Contracts.JobService;

namespace TripService.Saga
{
    public class ReservationStateMachine: MassTransitStateMachine<ReservationState>
    {
        public State WaitingForHotelResponse { get; private set; }
        public ReservationStateMachine()
        {
            InstanceState(x => x.CurrentState);
            Event(() => ReserveTrip, x
                => x.CorrelateById(state => state.CorrelationId,context=>context.Message.ReservationId)
                    .SelectId(ctx => ctx.Message.ReservationId));

            Event(() => ReserveHotelSuccessResponse, x
                => x.CorrelateById(state => state.CorrelationId, context => context.Message.ReservationId));
            
            Event(() => ReserveHotelFailureResponse, x
                => x.CorrelateById(state => state.CorrelationId, context => context.Message.ReservationId));

            Initially(
                    When(ReserveTrip)
                        .Publish(ctx =>new ReserveHotelQuery()
                        {
                            ReservationId = ctx.Saga.CorrelationId,
                            ReserveTripOfferParameters = ctx.Message.ReserveTripOfferParameters
                        })
                        .ThenAsync(async context =>
                        {
                            if (!context.TryGetPayload(out SagaConsumeContext<ReservationState, ReserveTripQuery> payload))
                                throw new Exception("Unable to retrieve required payload for callback data.");

                            context.Saga.RequestId = payload.RequestId;
                            context.Saga.ResponseAddress = payload.ResponseAddress;
                            
                        }).TransitionTo(WaitingForHotelResponse));
            During(WaitingForHotelResponse,
                When(ReserveHotelSuccessResponse)
                    .ThenAsync(ctx =>
                    {
                        return Console.Out.WriteLineAsync(
                            $"Sukces rezerwacji hotelu dla id: {ctx.Message.ReservationId}");
                    }).Finalize(),
                    When(ReserveHotelFailureResponse)
                        .ThenAsync(async ctx =>
                        {
                            var endpoint = await ctx.GetSendEndpoint(ctx.Saga.ResponseAddress);
                            await endpoint.Send(new ReserveTripResponse()
                            {
                                XD = 33
                            }, r => r.RequestId = ctx.Saga.RequestId);
                            await Console.Out.WriteLineAsync($"Błąd rezerwacji hotelu dla id: {ctx.Message.ReservationId}");
                        }).Finalize());
            SetCompletedWhenFinalized();
        }
        
        public Event<ReserveTripQuery> ReserveTrip { get; private set; }
        public Event<ReserveHotelSuccessResponse> ReserveHotelSuccessResponse { get; private set; }
        public Event<ReserveHotelFailureResponse> ReserveHotelFailureResponse { get; private set; }
    }
}