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
        public State WaitingForUserPayment { get; private set; }
        public State WaitingForPaymentServiceResponse { get; private set; }

        public ReservationStateMachine() 
        {
            
            InstanceState(x => x.CurrentState);
            ConfigureEvents();
            Initially(SubmitOrderHandler());
            
            Schedule(() => PaymentTimeout,
                x => x.TimeoutId,
                x => x.Delay = TimeSpan.FromSeconds(60));
            
            During(WaitingForHotelResponse,WaitingForHotelResponseSuccessHandler(),
                WaitingForHotelResponseFailureHandler());
            During(HotelReservationSucceded,HotelReservationSucceedReserveTransportSuccessResponseHandler(),
                HotelReservationSucceedReserveTransportFailureResponse());

            During(WaitingForUserPayment,WaitingForUserPaymentPaymentQuery(),WaitingForUserPaymentPaymentExpired());
            
            During(WaitingForPaymentServiceResponse,WaitingForPaymentServiceResponsePaymentRejected(),
                WaitingForPaymentServiceResponsePaymentAccepted());
            SetCompletedWhenFinalized();
        } 
        private void ConfigureEvents()
        {
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
            
            Event(() => PaymentQuery, x
                => x.CorrelateById(state => state.CorrelationId, context => context.Message.ReservationId));
            
            Event(() => PaymentExpired, x
                => x.CorrelateById(state => state.CorrelationId, context => context.Message.ReservationId));
            
            Event(() => PaymentForTripAcceptedResponse, x
                => x.CorrelateById(state => state.CorrelationId, context => context.Message.ReservationId));
            
            Event(() => PaymentForTripRejectedResponse, x
                => x.CorrelateById(state => state.CorrelationId, context => context.Message.ReservationId));
        }

        private EventActivityBinder<ReservationState,ReserveTripQuery> SubmitOrderHandler() =>
            When(ReserveTrip)
                .Publish(ctx => new ReserveHotelQuery()
                {
                    ReservationId = ctx.Saga.CorrelationId,
                    ReserveTripOfferParameters = ctx.Message.ReserveTripOfferParameters
                })
                .ThenAsync(async context =>
                {
                    context.Saga.ReserveTripOfferParameters = context.Message.ReserveTripOfferParameters;
                    context.Saga.Username = context.Message.ReserveTripOfferParameters.Username;
                    if (!context.TryGetPayload(out SagaConsumeContext<ReservationState, ReserveTripQuery> payload))
                        throw new Exception("Unable to retrieve required payload for callback data.");

                    context.Saga.RequestId = payload.RequestId;
                    context.Saga.ResponseAddress = payload.ResponseAddress;

                }).TransitionTo(WaitingForHotelResponse);

        private EventActivityBinder<ReservationState,ReserveHotelSuccessResponse>  WaitingForHotelResponseSuccessHandler() =>

            When(ReserveHotelSuccessResponse)
                .ThenAsync(async ctx =>
                {
                    ctx.Saga.hotelPrice = ctx.Message.Price;
                    if (ctx.Saga.ReserveTripOfferParameters.PromoCode == "PROMOCJA")
                    {
                        ctx.Saga.hotelPrice = (int) (ctx.Saga.hotelPrice * 0.9);
                    }
                    ctx.Saga.City = ctx.Message.City;
                    ctx.Saga.Country = ctx.Message.Country;
                    ctx.Saga.HotelName = ctx.Message.HotelName;
                    ctx.Saga.RoomTypeName = ctx.Message.ReservedRoomName;
                    ctx.Saga.FoodTypeName = ctx.Message.FoodType;
                    
                    await Console.Out.WriteLineAsync($"Sukces rezerwacji hotelu dla id: {ctx.Message.ReservationId}");

                })
                .Publish(ctx => new ReserveTransportQuery()
                {
                    ReturnTransportID = ctx.Saga.ReserveTripOfferParameters.TransportToId,
                    DepartueTransportID = ctx.Saga.ReserveTripOfferParameters.TransportFromId,
                    Places = ctx.Saga.ReserveTripOfferParameters.Adults +
                             ctx.Saga.ReserveTripOfferParameters.ChildrenUnder3 +
                             ctx.Saga.ReserveTripOfferParameters.ChildrenUnder10 +
                             ctx.Saga.ReserveTripOfferParameters.ChildrenUnder18,
                    ReservationId = ctx.Saga.CorrelationId,
                    ReserveTripOfferParameters = ctx.Saga.ReserveTripOfferParameters,
                    HotelCity = ctx.Saga.City
                })
                .TransitionTo(HotelReservationSucceded);
       private EventActivityBinder<ReservationState,ReserveHotelFailureResponse> WaitingForHotelResponseFailureHandler() => 
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
            }).Finalize();
       
       private EventActivityBinder<ReservationState, ReserveTransportSuccessResponse>
           HotelReservationSucceedReserveTransportSuccessResponseHandler() =>
           When(ReserveTransportSuccessResponse).ThenAsync(ctx =>
               {
                   ctx.Saga.transportPrice = ctx.Message.Price;
                   if (ctx.Saga.ReserveTripOfferParameters.PromoCode == "PROMOCJA")
                   {
                       ctx.Saga.transportPrice = (int) (ctx.Saga.transportPrice * 0.9);
                   }
                   ctx.Saga.TransportTypeName = ctx.Message.TransportTypeName;
                   return Console.Out.WriteLineAsync(
                       $"Sukces rezerwacji Transportu dla id: {ctx.Message.ReservationId}");
               })
               .ThenAsync(async ctx =>
               {
                   var endpoint = await ctx.GetSendEndpoint(ctx.Saga.ResponseAddress);
                   await endpoint.Send(new ReserveTripResponse()
                   {
                       Success = true,
                       Price = ctx.Saga.hotelPrice + ctx.Saga.transportPrice,
                       ReservationId = ctx.Saga.CorrelationId
                   }, r => r.RequestId = ctx.Saga.RequestId);
               })
               .Schedule(PaymentTimeout, ctx => new PaymentExpired()
               {
                   ReservationId = ctx.Saga.CorrelationId
               })
               .TransitionTo(WaitingForUserPayment);
       
       private EventActivityBinder<ReservationState, ReserveTransportFailureResponse>
           HotelReservationSucceedReserveTransportFailureResponse() =>
           When(ReserveTransportFailureResponse)
               .ThenAsync(ctx =>
                   Console.Out.WriteLineAsync($"Błąd rezerwacji Transportu dla id: {ctx.Message.ReservationId}"))
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
               .Finalize();

       private EventActivityBinder<ReservationState,PaymentQuery> WaitingForUserPaymentPaymentQuery() =>
           When(PaymentQuery)
               .Unschedule(PaymentTimeout)
               .ThenAsync(ctx =>
               {
                   if (!ctx.TryGetPayload(
                       out SagaConsumeContext<ReservationState, PaymentQuery> payload))
                       throw new Exception("Unable to retrieve required payload for callback data.");

                   ctx.Saga.RequestId = payload.RequestId;
                   ctx.Saga.ResponseAddress = payload.ResponseAddress;
                   return Console.Out.WriteLineAsync(
                       $"Otrzymano żądanie płatności dla id: {ctx.Message.ReservationId}");
               })
               .IfElse(ctx => ctx.Saga.Username == ctx.Message.Username,
                   x =>
                       x.ThenAsync(async context =>
                       {
                           Console.Out.WriteLineAsync(
                               $"Username poprawny dla rezerwacji {context.Message.ReservationId}");

                       }).Publish(ctx => new PayForTripQuery()
                       {
                           ReservationId = ctx.Saga.CorrelationId,
                           CardNumber = ctx.Message.CardNumber,
                           Price = ctx.Saga.hotelPrice + ctx.Saga.transportPrice
                       }).TransitionTo(WaitingForPaymentServiceResponse),
                   x =>
                       x.ThenAsync(async ctx =>
                       {
                           Console.Out.WriteLineAsync(
                               $"Username niepoprawny dla rezerwacji {ctx.Message.ReservationId}");
                           var endpoint = await ctx.GetSendEndpoint(ctx.Saga.ResponseAddress);
                           await endpoint.Send(new PaymentResponse()
                           {
                               Success = false,
                               Timeout = false,
                               ReservationId = ctx.Saga.CorrelationId
                           }, r => r.RequestId = ctx.Saga.RequestId);
                       }).Publish(ctx => new RollbackHotelReservationQuery()
                       {
                           TripReservationId = ctx.Saga.CorrelationId
                       }).Publish(ctx => new RollbackTransportReservationQuery()
                       {
                           TripReservationId = ctx.Saga.CorrelationId
                           
                       }).Finalize());

       private EventActivityBinder<ReservationState, PaymentExpired> WaitingForUserPaymentPaymentExpired() =>
           When(PaymentExpired)
               .ThenAsync(ctx =>
               {
                   return Console.Out.WriteLineAsync(
                       $"Czas na płatnosc minal dla id: {ctx.Message.ReservationId}");
               })
               .Publish(ctx => new RollbackTransportReservationQuery()
               {
                   TripReservationId = ctx.Saga.CorrelationId
               })
               .Publish(ctx => new RollbackHotelReservationQuery()
               {
                   TripReservationId = ctx.Saga.CorrelationId
               })
               .Finalize();

       private EventActivityBinder<ReservationState,PaymentForTripRejectedResponse> WaitingForPaymentServiceResponsePaymentRejected() =>
           When(PaymentForTripRejectedResponse)
               .ThenAsync(ctx =>
               {
                   return Console.Out.WriteLineAsync(
                       $"Błąd płatności dla id: {ctx.Message.ReservationId}");
               })
               .Publish(ctx => new RollbackHotelReservationQuery()
               {
                   TripReservationId = ctx.Saga.CorrelationId
               })
               .Publish(ctx => new RollbackTransportReservationQuery()
               {
                   TripReservationId = ctx.Saga.CorrelationId
               })
               .ThenAsync(async ctx =>
               {
                   var endpoint = await ctx.GetSendEndpoint(ctx.Saga.ResponseAddress);
                   await endpoint.Send(new PaymentResponse()
                   {
                       Success = false,
                       Timeout = false,
                       ReservationId = ctx.Saga.CorrelationId
                   }, r => r.RequestId = ctx.Saga.RequestId);
               })
               .Finalize();
       
       private EventActivityBinder<ReservationState,PaymentForTripAcceptedResponse> WaitingForPaymentServiceResponsePaymentAccepted() =>
           When(PaymentForTripAcceptedResponse)
               .ThenAsync(ctx =>
               {
                   return Console.Out.WriteLineAsync(
                       $"Sukces płatności dla id: {ctx.Message.ReservationId}");
               })
               .Publish(ctx => new ConfirmHotelOrderQuery()
               {
                   ReservationId = ctx.Saga.CorrelationId
               })
               .Publish(ctx => new ConfirmTransportOrderQuery()
                   {
                       ReservationId = ctx.Saga.CorrelationId
                   }
               )
               .Publish(ctx => new CreateUserTripQuery()
               {
                   id = ctx.Saga.CorrelationId,
                   City = ctx.Saga.City,
                   Country = ctx.Saga.Country,
                   FoodTypeName = ctx.Saga.FoodTypeName,
                   HotelName = ctx.Saga.HotelName,
                   Persons = ctx.Saga.ReserveTripOfferParameters.Adults
                             +ctx.Saga.ReserveTripOfferParameters.ChildrenUnder3
                             +ctx.Saga.ReserveTripOfferParameters.ChildrenUnder10
                             +ctx.Saga.ReserveTripOfferParameters.ChildrenUnder18,
                   RoomTypeName = ctx.Saga.RoomTypeName,
                   TransportTypeName = ctx.Saga.TransportTypeName,
                   username = ctx.Saga.Username,
                   StartDate = ctx.Saga.ReserveTripOfferParameters.StartDate,
                   EndDate = ctx.Saga.ReserveTripOfferParameters.EndDate
               })
               .ThenAsync(async ctx =>
               {
                   var endpoint = await ctx.GetSendEndpoint(ctx.Saga.ResponseAddress);
                   await endpoint.Send(new PaymentResponse()
                   {
                       Success = true,
                       Timeout = false,
                       ReservationId = ctx.Saga.CorrelationId
                   }, r => r.RequestId = ctx.Saga.RequestId);
               })
               .Finalize();
    
        
        public Schedule<ReservationState, PaymentExpired> PaymentTimeout { get; set; }
        public Event<PaymentExpired> PaymentExpired { get; set; }
        public Event<ReserveTripQuery> ReserveTrip { get; private set; }
        public Event<ReserveHotelSuccessResponse> ReserveHotelSuccessResponse { get; private set; }
        public Event<ReserveHotelFailureResponse> ReserveHotelFailureResponse { get; private set; }
        public Event<ReserveTransportSuccessResponse> ReserveTransportSuccessResponse { get; private set; }
        public Event<ReserveTransportFailureResponse> ReserveTransportFailureResponse { get; private set; }
        public Event<PaymentForTripAcceptedResponse> PaymentForTripAcceptedResponse { get; private set; }
        public Event<PaymentForTripRejectedResponse> PaymentForTripRejectedResponse { get; private set; }
        public Event<PaymentQuery> PaymentQuery { get; private set; }
    }
}