using System;
using System.Threading;
using System.Threading.Tasks;
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
        
        [HttpGet(Name = "GetTrip")]
        public async Task<GetTripsRespond> Index(CancellationToken cancellationToken)
        {
            var response =
                await _client.GetResponse<GetTripsRespond>(new GetTripsQuery {Country = "Poland"}, cancellationToken);
            return response.Message;
        }
    }
}