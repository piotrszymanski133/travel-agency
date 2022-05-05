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
    public class ConfirmHotelOrderConsumer: IConsumer<ConfirmTransportOrderQuery>
    {
        private readonly ILogger<GetTransportOffersQuery> _logger;
        private ITransportRepository _repository;

        public ConfirmHotelOrderConsumer(ILogger<GetTransportOffersQuery> logger, ITransportRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ConfirmTransportOrderQuery> context)
        {
            var command = context.Message;
            _repository.ConfirmTransport(command.ReservationId);
        }
    }
}