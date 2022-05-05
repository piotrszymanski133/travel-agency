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
            if (new Random().Next() % 10 == 0)
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