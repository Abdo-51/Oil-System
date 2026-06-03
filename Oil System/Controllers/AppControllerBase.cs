using Microsoft.AspNetCore.Mvc;
using Oil_System.Contract.BaseResponse;
using System.Net;

namespace Oil_System.Controllers
{
    [ApiController]
    public class AppControllerBase : ControllerBase
    {
        public ObjectResult GenericResult<T>(BaseResponse<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response);
                case HttpStatusCode.NotFound:
                    return NotFound(response);
                case HttpStatusCode.Created:
                    return Created(string.Empty, response);
                case HttpStatusCode.Accepted:
                    return Accepted(response);
                default:
                    return StatusCode(500, response);
            }
        }
    }
}
