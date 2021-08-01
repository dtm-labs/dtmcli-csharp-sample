using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DtmTccSample.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class TransInController : ControllerBase
    {
        private readonly ILogger<TransInController> _logger;

        public TransInController(ILogger<TransInController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public  RestResult Try([FromQuery]string gid, [FromQuery]string trans_type,
            [FromQuery]string branch_id,[FromQuery]string branch_type, [FromBody]TransRequest body)
        {
            return new RestResult() { Result = "SUCCESS" };
        }

        [HttpPost]
        public  RestResult Confirm([FromQuery] string gid, [FromQuery] string trans_type,
           [FromQuery] string branch_id, [FromQuery] string branch_type, [FromBody] TransRequest body)
        {
            return new RestResult() { Result = "SUCCESS" };
        }

        [HttpPost]
        public RestResult Cancel([FromQuery] string gid, [FromQuery] string trans_type,
           [FromQuery] string branch_id, [FromQuery] string branch_type, [FromBody] TransRequest body)
        {
            return new RestResult() { Result = "SUCCESS" };
        }
    }
}
