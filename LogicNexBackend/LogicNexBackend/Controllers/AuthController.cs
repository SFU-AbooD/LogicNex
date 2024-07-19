using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogicNexBackend.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
