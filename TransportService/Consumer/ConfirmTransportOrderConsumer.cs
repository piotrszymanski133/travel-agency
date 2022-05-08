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
    public class ConfirmTransportOrderConsumer: IConsumer<ConfirmTransportOrderQuery>
    {
        private ITransportRepository _repository;

        public ConfirmTransportOrderConsumer(ITransportRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ConfirmTransportOrderQuery> context)
        {
            var command = context.Message;
            _repository.ConfirmTransport(command.ReservationId);
            Console.WriteLine($"Transport purchase confirmed for id {command.ReservationId}");
        }
    }
}