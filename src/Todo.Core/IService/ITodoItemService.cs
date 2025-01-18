using Todo.Core.Entities.Request;
using Todo.Core.Entities.Response;

namespace Todo.Core.IService
{
    public interface ITodoItemService
    {
        Task<ServiceResponse<TodoItemBody>> CreateTodoItem(CreateTodoItemBody body);
        Task<ServiceResponse<TodoItemBody>> UpdateTodoItem(UpdateTodoItemBody body);
        Task<ServiceResponse<TodoItemBody>> DeleteTodoItem(Guid id);
        Task<ServiceResponse<PaginationResponse<TodoItemBody>>> GetAllTodoItemsByGroupId(Guid groupId, int limit, int offset);
        Task<ServiceResponse<TodoItemBody>> GetTodoItemById(Guid id);
    }
}