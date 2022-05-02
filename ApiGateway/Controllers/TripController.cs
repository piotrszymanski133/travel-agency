using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ApiGateway.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using CommonComponents;

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
    }
}