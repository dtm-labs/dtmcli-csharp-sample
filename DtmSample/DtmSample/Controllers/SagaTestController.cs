using Dtmcli;
using DtmSample.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DtmSample.Controllers
{
    /// <summary>
    /// SAGA 示例
    /// </summary>
    [ApiController]
    [Route("/api")]
    public class SagaTestController : ControllerBase
    {
        private readonly ILogger<SagaTestController> _logger;
        private readonly IDtmClient _dtmClient;
        private readonly AppSettings _settings;

        public SagaTestController(ILogger<SagaTestController> logger, IOptions<AppSettings> optionsAccs, IDtmClient dtmClient)
        {
            _logger = logger;
            _settings = optionsAccs.Value;
            _dtmClient = dtmClient;
        }

        /// <summary>
        /// SAGA 常规成功
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("saga")]
        public async Task<IActionResult> Saga(CancellationToken cancellationToken)
        {
            var gid = await _dtmClient.GenGid(cancellationToken);
            var saga = new Saga(this._dtmClient, gid)
                .Add(_settings.BusiUrl + "/TransOut", _settings.BusiUrl + "/TransOutRevert", new TransRequest("1", -30))
                .Add(_settings.BusiUrl + "/TransIn", _settings.BusiUrl + "/TransInRevert", new TransRequest("2", 30))
                ;

            var flag = await saga.Submit(cancellationToken);

            _logger.LogInformation("result gid is {0}, flag is {1}", gid, flag);

            return Ok(TransResponse.BuildSucceedResponse());
        }

        /// <summary>
        /// SAGA 失败回滚
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("saga-cancel")]
        public async Task<IActionResult> SagaCancel(CancellationToken cancellationToken)
        {
            var gid = await _dtmClient.GenGid(cancellationToken);
            var saga = new Saga(this._dtmClient, gid)
                .Add(_settings.BusiUrl + "/TransOutError", _settings.BusiUrl + "/TransOutRevert", new TransRequest("1", -30))
                .Add(_settings.BusiUrl + "/TransIn", _settings.BusiUrl + "/TransInRevert", new TransRequest("2", 30))
                ;

            var flag = await saga.Submit(cancellationToken);

            _logger.LogInformation("result gid is {0}, flag is {1}", gid, flag);

            return Ok(TransResponse.BuildSucceedResponse());
        }

        /// <summary>
        /// SAGA 等待结果
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("saga-waitresult")]
        public async Task<IActionResult> SagaWaitResult(CancellationToken cancellationToken)
        {
            var gid = await _dtmClient.GenGid(cancellationToken);
            var saga = new Saga(this._dtmClient, gid)
                .Add(_settings.BusiUrl + "/TransOut", _settings.BusiUrl + "/TransOutRevert", new TransRequest("1", -30))
                .Add(_settings.BusiUrl + "/TransIn", _settings.BusiUrl + "/TransInRevert", new TransRequest("2", 30))
                .EnableWaitResult()
                ;

            var flag = await saga.Submit(cancellationToken);

            _logger.LogInformation("result gid is {0}, flag is {1}", gid, flag);

            return Ok(TransResponse.BuildSucceedResponse());
        }

        /// <summary>
        /// SAGA 并发
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("saga-multi")]
        public async Task<IActionResult> SagaMulti(CancellationToken cancellationToken)
        {
            var gid = await _dtmClient.GenGid(cancellationToken);
            var saga = new Saga(this._dtmClient, gid)
                .Add(_settings.BusiUrl + "/TransOut", _settings.BusiUrl + "/TransOutRevert", new TransRequest("1", -30))
                .Add(_settings.BusiUrl + "/TransOut", _settings.BusiUrl + "/TransOutRevert", new TransRequest("1", -30))
                .Add(_settings.BusiUrl + "/TransIn", _settings.BusiUrl + "/TransInRevert", new TransRequest("2", 30))
                .Add(_settings.BusiUrl + "/TransIn", _settings.BusiUrl + "/TransInRevert", new TransRequest("2", 30))
                .EnableConcurrent()
                .AddBranchOrder(2, new List<int> { 0, 1 })
                .AddBranchOrder(3, new List<int> { 0, 1 })
                ;

            var flag = await saga.Submit(cancellationToken);

            _logger.LogInformation("result gid is {0}, flag is {1}", gid, flag);

            return Ok(TransResponse.BuildSucceedResponse());
        }

        /// <summary>
        /// SAGA 异常触发子事务屏障
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("saga-barrier")]
        public async Task<IActionResult> SagaBarrier(CancellationToken cancellationToken)
        {
            var gid = await _dtmClient.GenGid(cancellationToken);
            var saga = new Saga(this._dtmClient, gid)
                .Add(_settings.BusiUrl + "/barrierTransOutSaga", _settings.BusiUrl + "/barrierTransOutSagaRevert", new TransRequest("1", -30))
                .Add(_settings.BusiUrl + "/barrierTransInSaga", _settings.BusiUrl + "/barrierTransInSagaRevert", new TransRequest("2", 30))
                ;

            var flag = await saga.Submit(cancellationToken);

            _logger.LogInformation("result gid is {0}, flag is {1}", gid, flag);

            return Ok(TransResponse.BuildSucceedResponse());
        }
    }
}
