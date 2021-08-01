using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtmcli;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace DtmTccSample.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        TccGlobalTransaction globalTransaction;
        ILogger logger;

        public HomeController(TccGlobalTransaction transaction,ILoggerFactory loggerFactory)
        {
            globalTransaction = transaction;
            logger = loggerFactory.CreateLogger<HomeController>();
        }

        [HttpPost]
        public async Task<RestResult> Demo()
        {
            var svc = "http://192.168.5.9:5000/api";
            TransRequest request = new TransRequest() { Amount = 30 };
            var cts = new CancellationTokenSource();
            try
            {
                await globalTransaction.Excecute(async (tcc) =>
                {
                    var res1 = await tcc.CallBranch(request, svc + "/TransOut/Try", svc + "/TransOut/Confirm", svc + "/TransOut/Cancel", cts.Token);
                    var res2 = await tcc.CallBranch(request, svc + "/TransIn/Try", svc + "/TransIn/Confirm", svc + "/TransIn/Cancel",cts.Token);
                    logger.LogInformation($"tcc returns: {res1}-{res2}");
                }, cts.Token);
                
            }
            catch(Exception ex)
            {

            }
            return new RestResult() { Result = "SUCCESS" };
        }
    }
}
