using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransportService.Models;
using TransportService.Services;
using TripService.Repository;
using Transport = CommonComponents.Models.Transport;

namespace TransportService.Consumer
{
    public class ReserveTransportQueryConsumer : IConsumer<ReserveTransportQuery>
    {
        private readonly ILogger<GetTransportOffersQuery> _logger;
        private ITransportRepository _repository;

        public ReserveTransportQueryConsumer(ILogger<GetTransportOffersQuery> logger, ITransportRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ReserveTransportQuery> context)
        {
            var command = context.Message;

            if (command.DepartueTransportID == -1 || command.ReturnTransportID == -1)
            {
                await context.Publish(new ReserveTransportSuccessResponse()
                {
                    Price = 0,
                    ReservationId = command.ReservationId,
                    TransportTypeName = "Own"
                });
            }
            else
            {
                var ret = _repository.ReserveTransport(command.DepartueTransportID, command.ReturnTransportID,
                    command.Places, command.ReservationId,
                    command.ReserveTripOfferParameters.StartDate,
                    command.ReserveTripOfferParameters.EndDate,command.ReserveTripOfferParameters.Username,command.HotelCity);
                var succes = ret.Item2;

                if (succes)
                {
                    var transport = _repository.GetTransport(context.Message.DepartueTransportID);
                    int price = PriceCalculator.CalculateTransportOfferPrice(transport.Transporttype,
                        context.Message.Places, transport.DestinationPlaces.Country);
                    await context.Publish(new ReserveTransportSuccessResponse()
                    {
                        Price = price,
                        ReservationId = command.ReservationId,
                        TransportTypeName = ret.Item1
                    });
                }
                else
                {
                    await context.Publish(new ReserveTransportFailureResponse()
                    {
                        ReservationId = command.ReservationId
                    });
                }
            }
        }
    }
}