using System.Threading.Tasks;
using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LastChangesController: ControllerBase
    {
        private ILastChangesService _lastChangesService;

        public LastChangesController(ILastChangesService lastChangesService)
        {
            _lastChangesService = lastChangesService;
        }

        
        [HttpGet]
        [Route("GetLastChanges")]
        public  LastChangesList GetLastChanges()
        {
            return new LastChangesList(_lastChangesService._changeHotel, _lastChangesService._changeTransport);;
        }
    }
}