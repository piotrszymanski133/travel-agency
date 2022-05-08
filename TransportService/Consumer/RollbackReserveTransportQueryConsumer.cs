using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonComponents;
using CommonComponents.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransportService.Models;
using TripService.Repository;
using Transport = CommonComponents.Models.Transport;

namespace TransportService.Consumer
{
    public class RollbackReserveTransportQueryConsumer: IConsumer<RollbackTransportReservationQuery>
    {
        private readonly ILogger<GetTransportOffersQuery> _logger;
        private ITransportRepository _repository;

        public RollbackReserveTransportQueryConsumer(ILogger<GetTransportOffersQuery> logger, ITransportRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<RollbackTransportReservationQuery> context)
        {
            var command = context.Message;
            _repository.RollbackReserveTransport(command.TripReservationId);
            Console.WriteLine($"Transport reservation rollbacked for id {command.TripReservationId}");

        }
    }
}