using System;
using System.Threading.Tasks;
using CommonComponents;
using MassTransit;

namespace PaymentService.Consumers
{
    public class PayForTripConsumer : IConsumer<PayForTripQuery>
    {
        public Task Consume(ConsumeContext<PayForTripQuery> context)
        {
            if (context.Message.CardNumber.EndsWith("0"))
            {
                context.Publish(new PaymentForTripRejectedResponse()
                {
                    ReservationId = context.Message.ReservationId
                });
            }
            else
            {
                context.Publish(new PaymentForTripAcceptedResponse()
                {
                    ReservationId = context.Message.ReservationId
                });
            }

            return Task.CompletedTask;
        }
    }
}