using System.Threading.Tasks;
using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;
using CommonComponents.Models;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Index([FromBody] LoginUser loginUser)
        {
            if (_userService.checkIfUserExisted(loginUser))
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}