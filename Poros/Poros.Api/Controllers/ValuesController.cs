using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Poros.Api.Controllers
{
    [Authorize(Policy = "ApiReader")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [Authorize(Policy = "Passenger")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}
