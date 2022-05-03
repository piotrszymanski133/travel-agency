using System;
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
        private IRequestClient<GetTripOfferQuery> _tripOfferclient;

        public TripController(IRequestClient<GetTripsQuery> tripsClient, IRequestClient<GetTripOfferQuery> tripOfferclient)
        {
            _tripOfferclient = tripOfferclient;
            _tripsClient = tripsClient;
        }

        [HttpGet]
        public async Task<GetTripsResponse> Index([FromQuery] TripParameters tripParameters)
        {
            var response =
                await _tripsClient.GetResponse<GetTripsResponse>(new GetTripsQuery {TripParameters = tripParameters});
            return response.Message;
        }
        
        [HttpGet]
        [Route("GetTrip")]
        public async Task<GetTripOfferResponse> GetTrip([FromQuery] TripOfferQueryParameters tripOfferQueryParameters)
        {
            var response = await _tripOfferclient.GetResponse<GetTripOfferResponse>(new GetTripOfferQuery
            {
                TripOfferQueryParameters = tripOfferQueryParameters
            });
            return response.Message;
            /*
            return new HotelOffer
            {
                Id = "123", Description = "Opis", DestinationCity = "Madryt", Rating = 4, Stars = 5,
                DestinationCountry = "Hiszpania"
            };
            */
        }
    }
}