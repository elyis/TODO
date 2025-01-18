using System.Net;
using Todo.Core.Entities.Request;
using Todo.Core.Entities.Response;
using Todo.Core.IRepository;
using Todo.Core.IService;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Todo.App.Service
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly ITodoGroupRepository _todoGroupRepository;
        private readonly ILogger<TodoItemService> _logger;

        public TodoItemService(
            ITodoItemRepository todoItemRepository,
            ITodoGroupRepository todoGroupRepository,
            ILogger<TodoItemService> logger)
        {
            _todoItemRepository = todoItemRepository;
            _todoGroupRepository = todoGroupRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<TodoItemBody>> CreateTodoItem(CreateTodoItemBody body)
        {
            var todoGroup = await _todoGroupRepository.GetAsync(body.TodoGroupId);
            if (todoGroup == null)
            {
                return new ServiceResponse<TodoItemBody>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new string[] { "Todo group not found" },
                    Body = null
                };
            }

            var todoItem = await _todoItemRepository.CreateTodoItem(body, todoGroup);
            return new ServiceResponse<TodoItemBody>
            {
                StatusCode = HttpStatusCode.OK,
                Body = todoItem.ToTodoItemBody()
            };
        }

        public async Task<ServiceResponse<TodoItemBody>> GetTodoItemById(Guid id)
        {
            var todoItem = await _todoItemRepository.GetTodoItemById(id);
            if (todoItem == null)
            {
                return new ServiceResponse<TodoItemBody>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = new string[] { "Todo item not found" },
                    Body = null
                };
            }

            return new ServiceResponse<TodoItemBody>
            {
                StatusCode = HttpStatusCode.OK,
                Body = todoItem.ToTodoItemBody()
            };
        }

        public async Task<ServiceResponse<TodoItemBody>> UpdateTodoItem(UpdateTodoItemBody body)
        {
            var todoItem = await _todoItemRepository.UpdateTodoItem(body);
            if (todoItem == null)
            {
                return new ServiceResponse<TodoItemBody>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = new string[] { "Todo item not found" },
                    Body = null
                };
            }

            return new ServiceResponse<TodoItemBody>
            {
                StatusCode = HttpStatusCode.OK,
                Body = todoItem.ToTodoItemBody()
            };
        }

        public async Task<ServiceResponse<TodoItemBody>> DeleteTodoItem(Guid id)
        {
            await _todoItemRepository.DeleteTodoItem(id);
            return new ServiceResponse<TodoItemBody>
            {
                StatusCode = HttpStatusCode.NoContent,
                Body = null
            };
        }

        public async Task<ServiceResponse<PaginationResponse<TodoItemBody>>> GetAllTodoItemsByGroupId(Guid groupId, int limit, int offset)
        {
            var todoItems = await _todoItemRepository.GetTodoItemsByGroupId(groupId, limit, offset);
            var totalCount = await _todoItemRepository.GetTotalCountByGroupIdAsync(groupId);
            return new ServiceResponse<PaginationResponse<TodoItemBody>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Body = new PaginationResponse<TodoItemBody>
                {
                    Items = todoItems.Select(ti => ti.ToTodoItemBody()),
                    Limit = limit,
                    Offset = offset,
                    TotalCount = totalCount
                }
            };
        }

    }
}