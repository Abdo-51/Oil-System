using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Oil_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OilsPacketController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("OilsPacketController is working!");
        }
    }
}
