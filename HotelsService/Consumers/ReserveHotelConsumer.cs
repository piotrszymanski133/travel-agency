using System;
using System.Threading.Tasks;
using CommonComponents;
using MassTransit;

namespace HotelsService.Consumers
{
    public class ReserveHotelConsumer : IConsumer<ReserveHotelQuery>
    {
        public async Task Consume(ConsumeContext<ReserveHotelQuery> context)
        {
            bool success = new Random().Next() % 2 == 0;
            if (success)
            {
                await context.Publish(new ReserveHotelSuccessResponse() 
                {
                    ReservationId = context.Message.ReservationId,
                });
                return;
            }
            else
            {
                await context.Publish(new ReserveHotelFailureResponse() 
                {
                    ReservationId = context.Message.ReservationId,
                });
            }

        }
    }
}