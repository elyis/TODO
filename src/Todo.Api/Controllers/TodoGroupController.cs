using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Core.Entities.Request;
using Todo.Core.Entities.Response;
using Todo.Core.IService;

namespace Todo.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class TodoGroupController : ControllerBase
    {
        private readonly ITodoGroupService _todoGroupService;

        public TodoGroupController(ITodoGroupService todoGroupService)
        {
            _todoGroupService = todoGroupService;
        }

        [HttpPost("todo-group"), Authorize]
        [SwaggerOperation("Создание группы задач")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Группа задач успешно создана", typeof(TodoGroupBody))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Ошибка валидации", typeof(IEnumerable<string>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Ошибка авторизации", typeof(IEnumerable<string>))]
        public async Task<IActionResult> CreateTodoGroup(
            CreateTodoGroupBody body,
            [FromHeader(Name = "Authorization")] string token)
        {
            var response = await _todoGroupService.CreateTodoGroup(body, token);
            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }

        [HttpGet("todo-group/{id}"), Authorize]
        [SwaggerOperation("Получение группы задач")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Группа задач успешно получена", typeof(TodoGroupBody))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Группа задач не найдена")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Ошибка авторизации")]
        public async Task<IActionResult> GetTodoGroup(Guid id)
        {
            var response = await _todoGroupService.GetTodoGroup(id);
            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }

        [HttpPatch("todo-group"), Authorize]
        [SwaggerOperation("Обновление названия группы задач")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Название группы задач успешно обновлено", typeof(TodoGroupBody))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Группа задач не найдена")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Ошибка авторизации")]
        public async Task<IActionResult> UpdateTodoGroupName(UpdateTodoGroupBody body)
        {
            var response = await _todoGroupService.UpdateTodoGroupName(body);
            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }

        [HttpDelete("todo-group/{id}"), Authorize]
        [SwaggerOperation("Удаление группы задач")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Группа задач успешно удалена")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Ошибка авторизации")]
        public async Task<IActionResult> DeleteTodoGroup(Guid id)
        {
            var response = await _todoGroupService.DeleteTodoGroup(id);
            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }

        [HttpGet("todo-groups"), Authorize]
        [SwaggerOperation("Получение всех групп задач")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Все группы задач успешно получены", typeof(PaginationResponse<TodoGroupBody>))]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Группы задач не найдены")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Ошибка авторизации")]
        public async Task<IActionResult> GetTodoGroups(
            [FromHeader(Name = "Authorization")] string token,
            [FromQuery, Range(1, 100)] int limit = 10,
            [FromQuery, Range(0, int.MaxValue)] int offset = 0)
        {
            var response = await _todoGroupService.GetTodoGroups(limit, offset, token);
            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }
    }
}