using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Core.Entities.Models;
using Todo.Core.Entities.Response;
using Todo.Core.IRepository;

namespace Todo.Api.Controllers
{
    [ApiController]
    [Route("api/request-logs")]
    public class RequestLogController : ControllerBase
    {
        private readonly IRequestLogRepository _requestLogRepository;

        public RequestLogController(IRequestLogRepository requestLogRepository)
        {
            _requestLogRepository = requestLogRepository;
        }

        [HttpGet, Authorize]
        [SwaggerOperation("Получение логов запросов")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Логи запросов", typeof(PaginationResponse<RequestLog>))]
        public async Task<IActionResult> GetRequestLogs(
            [FromQuery, Range(1, 100)] int limit = 10,
            [FromQuery, Range(0, int.MaxValue)] int offset = 0)
        {
            var requestLogs = await _requestLogRepository.GetRequestLogs(limit, offset);
            var totalCount = await _requestLogRepository.GetTotalCount();
            var response = new PaginationResponse<RequestLog>
            {
                Items = requestLogs,
                Limit = limit,
                Offset = offset,
                TotalCount = totalCount
            };
            return Ok(response);
        }
    }
}