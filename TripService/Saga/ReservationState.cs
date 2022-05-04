using System;
using CommonComponents.Models;
using MassTransit;


namespace TripService.Saga
{
    public class ReservationState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public string Username { get; set; }
        public Guid? RequestId { get; set; }
        public Uri? ResponseAddress { get; set; }
        public ReserveTripOfferParameters ReserveTripOfferParameters { get; set; }
    }
}