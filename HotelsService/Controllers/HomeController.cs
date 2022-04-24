using System;
using System.Threading.Tasks;
using HotelsService.Queries;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HotelsService.Controllers
{
    public class HomeController : ControllerBase
    {
        readonly IPublishEndpoint _publishEndpoint;

        public HomeController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        // GET
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _publishEndpoint.Publish<GetHotelsQuery>(new GetHotelsQuery
            {
                City = "Rakowiec",
                Country = "Poland"
            });

            return Ok();
        }
        
        [HttpGet]
        public IActionResult Ind()
        {
            return new BadRequestResult();
        }
    }
}