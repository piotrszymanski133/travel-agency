using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ApiGateway.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using CommonComponents;
using CommonComponents.Models;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripController : ControllerBase
    {
        private IRequestClient<GetTripsQuery> _tripsClient;
        private IRequestClient<GetTripOfferQuery> _tripOfferClient;
        private IRequestClient<ReserveTripQuery> _tripReservationClient;
        private IRequestClient<PaymentQuery> _tripPaymentClient;
        private IRequestClient<GetDestinationsQuery> _destinationsClient;
        private IRequestClient<GetUserTripsQuery> _userTripsClients;

        public TripController(IRequestClient<GetTripsQuery> tripsClient,
            IRequestClient<GetTripOfferQuery> tripOfferClient, IRequestClient<ReserveTripQuery> tripReservationClient,
            IRequestClient<PaymentQuery> tripPaymentClient, IRequestClient<GetDestinationsQuery> destinationsClient, IRequestClient<GetUserTripsQuery> userTripsClients)
        {
            _tripOfferClient = tripOfferClient;
            _tripReservationClient = tripReservationClient;
            _tripPaymentClient = tripPaymentClient;
            _destinationsClient = destinationsClient;
            _userTripsClients = userTripsClients;
            _tripsClient = tripsClient;
        }

        [HttpGet]
        public async Task<GetTripsResponse> Index([FromQuery] TripParameters tripParameters)
        {
            var response =
                await _tripsClient.GetResponse<GetTripsResponse>(new GetTripsQuery { TripParameters = tripParameters });
            return response.Message;
        }

        [HttpGet]
        [Route("GetTrip")]
        public async Task<GetTripOfferResponse> GetTrip([FromQuery] TripOfferQueryParameters tripOfferQueryParameters)
        {
            var response = await _tripOfferClient.GetResponse<GetTripOfferResponse>(new GetTripOfferQuery
            {
                TripOfferQueryParameters = tripOfferQueryParameters
            });
            return response.Message;
        }
        [HttpGet]
        [Route("GetUserTrips")]
        public async Task<GetUserTripsResponse> GetUserTrips([FromQuery] UserTripsQueryParemeters tripOfferQueryParameters)
        {
            var response = await _userTripsClients.GetResponse<GetUserTripsResponse>(new GetUserTripsQuery
            {
                Username = tripOfferQueryParameters.Username
            });
            
            return response.Message;
        }

        [HttpPost]
        [Route("ReserveTrip")]
        public async Task<ReserveTripResponse> ReserveTrip(
            [FromBody] ReserveTripOfferParameters reserveTripOfferParameters)
        {
            var response = await _tripReservationClient.GetResponse<ReserveTripResponse>(new ReserveTripQuery()
            {
                ReserveTripOfferParameters = reserveTripOfferParameters,
                ReservationId = Guid.NewGuid()
            });
            return response.Message;
        }

        [HttpPost]
        [Route("Payment")]
        public async Task<PaymentResponse> PayForTrip([FromBody] PaymentParameters paymentParameters)
        {
            var response = await _tripPaymentClient.GetResponse<PaymentResponse>(new PaymentQuery()
            {
                ReservationId = paymentParameters.ReservationId,
                CardNumber = paymentParameters.CardNumber,
                Username = paymentParameters.Username
            });
            return response.Message;
        }

        [HttpGet]
        [Route("GetDestinations")]
        public async Task<List<String>> GetDestinations()
        {
            var response = await _destinationsClient.GetResponse<GetDestinationsResponse>(new GetDestinationsQuery());
            return response.Message.Destinations;
        }
    }
}