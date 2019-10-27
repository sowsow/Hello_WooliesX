using Microsoft.AspNetCore.Mvc;

namespace WooliesX.WebApi.Controllers
{
    [Route("")]
    [ApiController]
    public class HealthController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200)]
        public string Ping()
        {
            return "Hello Woolies X from Rex Li";
        }
    }
}
