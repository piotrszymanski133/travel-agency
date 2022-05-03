using System;
using MassTransit;

namespace TripService.Saga
{
    public class ReservationState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
    }
}