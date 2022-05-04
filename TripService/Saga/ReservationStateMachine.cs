using System;
using MassTransit;
using CommonComponents;
using MassTransit.Contracts.JobService;

namespace TripService.Saga
{
    public class ReservationStateMachine: MassTransitStateMachine<ReservationState>
    {
        public State WaitingForHotelResponse { get; private set; }
        public State HotelReservationSucceded { get; private set; }
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
            
            Event(() => ReserveTransportSuccessResponse, x
                => x.CorrelateById(state => state.CorrelationId, context => context.Message.ReservationId));
            
            Event(() => ReserveTransportFailureResponse, x
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
                            context.Saga.ReserveTripOfferParameters = context.Message.ReserveTripOfferParameters;
                            
                            if (!context.TryGetPayload(out SagaConsumeContext<ReservationState, ReserveTripQuery> payload))
                                throw new Exception("Unable to retrieve required payload for callback data.");

                            context.Saga.RequestId = payload.RequestId;
                            context.Saga.ResponseAddress = payload.ResponseAddress;
                            
                        }).TransitionTo(WaitingForHotelResponse));
            During(WaitingForHotelResponse,
                When(ReserveHotelSuccessResponse)
                    .ThenAsync(async ctx =>
                    {
                        ctx.Saga.hotelPrice = ctx.Message.Price;
                        Console.Out.WriteLineAsync( $"Sukces rezerwacji hotelu dla id: {ctx.Saga.ReserveTripOfferParameters.Adults}");
                        
                    })
                    .Publish(ctx =>new ReserveTransportQuery()
                    {
                        ReturnTransportID = ctx.Saga.ReserveTripOfferParameters.TransportToId,
                        DepartueTransportID = ctx.Saga.ReserveTripOfferParameters.TransportFromId,
                        Places = ctx.Saga.ReserveTripOfferParameters.Adults +
                                 ctx.Saga.ReserveTripOfferParameters.ChildrenUnder3 +
                                 ctx.Saga.ReserveTripOfferParameters.ChildrenUnder10 +
                                 ctx.Saga.ReserveTripOfferParameters.ChildrenUnder18,
                        ReservationId = ctx.Saga.CorrelationId,
                        ReserveTripOfferParameters = ctx.Saga.ReserveTripOfferParameters
                    })
                    .TransitionTo(HotelReservationSucceded),
                When(ReserveHotelFailureResponse)
                    .ThenAsync(async ctx =>
                    {
                        var endpoint = await ctx.GetSendEndpoint(ctx.Saga.ResponseAddress);
                        await endpoint.Send(new ReserveTripResponse()
                        {
                            Success = false,
                            ReservationId = Guid.Empty
                        }, r => r.RequestId = ctx.Saga.RequestId);
                        await Console.Out.WriteLineAsync($"Błąd rezerwacji hotelu dla id: {ctx.Message.ReservationId}");
                    }).Finalize());
            During(HotelReservationSucceded, 
                When(ReserveTransportSuccessResponse).ThenAsync(ctx =>
            {
                ctx.Saga.transportPrice = ctx.Message.Price;
                return Console.Out.WriteLineAsync(
                    $"Sukces rezerwacji Transportu dla id: {ctx.Message.ReservationId}");
            }).ThenAsync(async ctx => 
                {
                    var endpoint = await ctx.GetSendEndpoint(ctx.Saga.ResponseAddress); 
                    await endpoint.Send(new ReserveTripResponse()
                    {
                        Success = true,
                        Price = ctx.Saga.hotelPrice + ctx.Saga.transportPrice,
                        ReservationId = ctx.Saga.CorrelationId
                    }, r => r.RequestId = ctx.Saga.RequestId);
                }),
                When(ReserveTransportFailureResponse)
                    .ThenAsync(ctx => 
                    { 
                        return Console.Out.WriteLineAsync($"Błąd rezerwacji Transportu dla id: {ctx.Message.ReservationId}"); 
                    })
                    .Publish(ctx => new RollbackHotelReservationQuery()
                    {
                        TripReservationId = ctx.Saga.CorrelationId
                    })
                    .ThenAsync(async ctx =>
                    { 
                        var endpoint = await ctx.GetSendEndpoint(ctx.Saga.ResponseAddress); 
                        await endpoint.Send(new ReserveTripResponse()
                        {
                            Success = false,
                            ReservationId = Guid.Empty
                        }, r => r.RequestId = ctx.Saga.RequestId);
                    })
                    .Finalize());
            SetCompletedWhenFinalized();
        }
        
        public Event<ReserveTripQuery> ReserveTrip { get; private set; }
        public Event<ReserveHotelSuccessResponse> ReserveHotelSuccessResponse { get; private set; }
        public Event<ReserveHotelFailureResponse> ReserveHotelFailureResponse { get; private set; }
        public Event<ReserveTransportSuccessResponse> ReserveTransportSuccessResponse { get; private set; }
        public Event<ReserveTransportFailureResponse> ReserveTransportFailureResponse { get; private set; }
    }
}