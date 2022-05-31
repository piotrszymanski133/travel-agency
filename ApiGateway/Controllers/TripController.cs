using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ApiGateway.Hubs;
using ApiGateway.Hubs.Clients;
using ApiGateway.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using CommonComponents;
using CommonComponents.Exceptions;
using CommonComponents.Models;
using MassTransit.Internals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

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
        private IRequestClient<GetNotificationAboutPopularCountryQuery> _notificationCountryClient;
        private IHubContext<NotificationHub, INotificationClient> _hub;

        public TripController(IRequestClient<GetTripsQuery> tripsClient,
            IRequestClient<GetTripOfferQuery> tripOfferClient, IRequestClient<ReserveTripQuery> tripReservationClient,
            IRequestClient<PaymentQuery> tripPaymentClient, IRequestClient<GetDestinationsQuery> destinationsClient,
            IRequestClient<GetUserTripsQuery> userTripsClients, IHubContext<NotificationHub, INotificationClient> hub,
            IRequestClient<GetNotificationAboutPopularCountryQuery> notificationCountryClient)
        {
            _tripOfferClient = tripOfferClient;
            _tripReservationClient = tripReservationClient;
            _tripPaymentClient = tripPaymentClient;
            _destinationsClient = destinationsClient;
            _userTripsClients = userTripsClients;
            _hub = hub;
            _notificationCountryClient = notificationCountryClient;
            _tripsClient = tripsClient;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Index([FromQuery] TripParameters tripParameters)
        {
            if (tripParameters.Adults <= 0 || tripParameters.ChildrenUnder3 < 0 || tripParameters.ChildrenUnder10 < 0 ||
                tripParameters.ChildrenUnder18 < 0 || tripParameters.StartDate < DateTime.Today ||
                tripParameters.EndDate < DateTime.Today || tripParameters.EndDate < tripParameters.StartDate)
            {
                return BadRequest($"Incorrect value of one of the parameters");
            }
            var response =
                await _tripsClient.GetResponse<GetTripsResponse>(new GetTripsQuery {TripParameters = tripParameters});
            
            return Ok(response.Message);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("GetTrip")]
        public async Task<IActionResult> GetTrip([FromQuery] TripOfferQueryParameters tripOfferQueryParameters)
        {
            if (tripOfferQueryParameters.Adults <= 0 || tripOfferQueryParameters.ChildrenUnder3 < 0 ||
                tripOfferQueryParameters.ChildrenUnder10 < 0 ||
                tripOfferQueryParameters.ChildrenUnder18 < 0 || tripOfferQueryParameters.StartDate < DateTime.Today ||
                tripOfferQueryParameters.EndDate < DateTime.Today ||
                tripOfferQueryParameters.EndDate < tripOfferQueryParameters.StartDate)
            {
                return BadRequest($"Incorrect value of one of the parameters");
            }

            var response = await _tripOfferClient.GetResponse<GetTripOfferResponse>(new GetTripOfferQuery
            {
                TripOfferQueryParameters = tripOfferQueryParameters
            });
            if (response.Message.TripOffer == null)
            {
                return BadRequest($"Incorrect value of one of the parameters");
            }
            return Ok(response.Message);
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
            if (reserveTripOfferParameters.Adults <= 0 ||
                reserveTripOfferParameters.ChildrenUnder3 < 0 ||
                reserveTripOfferParameters.ChildrenUnder10 < 0 ||
                reserveTripOfferParameters.ChildrenUnder18 < 0 || 
                reserveTripOfferParameters.StartDate < DateTime.Today ||
                reserveTripOfferParameters.EndDate < DateTime.Today ||
                reserveTripOfferParameters.EndDate < reserveTripOfferParameters.StartDate)
            {
                return new ReserveTripResponse()
                {
                    Success = false,
                    ReservationId = Guid.Empty
                };
            }
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
            if (paymentParameters.CardNumber.Length != 16 || !Regex.IsMatch(paymentParameters.CardNumber, @"^\d+$"))
            {
                return new PaymentResponse()
                {
                    Success = false,
                    Timeout = false,
                    ReservationId = paymentParameters.ReservationId
                };
            }

            try
            {
                var response = await _tripPaymentClient.GetResponse<PaymentResponse>(new PaymentQuery()
                {
                    ReservationId = paymentParameters.ReservationId,
                    CardNumber = paymentParameters.CardNumber,
                    Username = paymentParameters.Username
                });
                return response.Message;
            }
            catch (MassTransitException)
            {
                return new PaymentResponse()
                {
                    Success = false,
                    Timeout = true,
                    ReservationId = paymentParameters.ReservationId
                };
            }
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