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
        private IRequestClient<GetTripsQuery> _client;

        public TripController(IRequestClient<GetTripsQuery> client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<GetTripsRespond> Index([FromQuery] TripParameters tripParameters)
        {
            var response =
                await _client.GetResponse<GetTripsRespond>(new GetTripsQuery {TripParameters = tripParameters});
            return response.Message;
        }
        
        [HttpGet]
        [Route("GetTrip")]
        public async Task<Hotel> GetTrip()
        {
            return new Hotel
            {
                Id = "123", Description = "Opis", DestinationCity = "Madryt", Rating = 4, Stars = 5,
                DestinationCountry = "Hiszpania"
            };
        }
    }
}