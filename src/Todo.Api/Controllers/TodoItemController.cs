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
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;

        public TodoItemController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        [HttpPost("todo-item"), Authorize]
        [SwaggerOperation("Создание новой задачи")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Задача успешно создана")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Ошибка валидации или неверный идентификатор группы", typeof(string))]
        public async Task<IActionResult> CreateTodoItem(CreateTodoItemBody body)
        {
            var response = await _todoItemService.CreateTodoItem(body);
            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }

        [HttpPatch("todo-item"), Authorize]
        [SwaggerOperation("Обновление задачи (null поля поля не обновляются)")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Задача успешно обновлена", typeof(TodoItemBody))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Ошибка валидации или неверный идентификатор задачи", typeof(string))]
        public async Task<IActionResult> UpdateTodoItem(UpdateTodoItemBody body)
        {
            var response = await _todoItemService.UpdateTodoItem(body);
            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }

        [HttpDelete("todo-item/{id}"), Authorize]
        [SwaggerOperation("Удаление задачи")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Задача успешно удалена")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Ошибка валидации", typeof(string))]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            var response = await _todoItemService.DeleteTodoItem(id);
            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode);

            return StatusCode((int)response.StatusCode, response.Errors);
        }

        [HttpGet("todo-items"), Authorize]
        [SwaggerOperation("Получение всех задач группы")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Задачи успешно получены", typeof(PaginationResponse<TodoItemBody>))]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Задачи не найдены")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Ошибка валидации или неверный идентификатор группы", typeof(string))]
        public async Task<IActionResult> GetAllTodoItemsByGroupId(
            [FromQuery] Guid groupId,
            [FromQuery, Range(1, 100)] int limit = 10,
            [FromQuery, Range(0, int.MaxValue)] int offset = 0)
        {
            var response = await _todoItemService.GetAllTodoItemsByGroupId(groupId, limit, offset);
            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }

        [HttpGet("todo-item/{id}"), Authorize]
        [SwaggerOperation("Получение задачи по идентификатору")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Задача успешно получена", typeof(TodoItemBody))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Задача не найдена")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Ошибка валидации", typeof(string))]
        public async Task<IActionResult> GetTodoItemById(Guid id)
        {
            var response = await _todoItemService.GetTodoItemById(id);
            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }
    }
}