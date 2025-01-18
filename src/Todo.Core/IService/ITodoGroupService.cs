using Todo.Core.Entities.Request;
using Todo.Core.Entities.Response;

namespace Todo.Core.IService
{
    public interface ITodoGroupService
    {
        Task<ServiceResponse<TodoGroupBody>> CreateTodoGroup(CreateTodoGroupBody body, string token);
        Task<ServiceResponse<TodoGroupBody>> GetTodoGroup(Guid id);
        Task<ServiceResponse<TodoGroupBody>> UpdateTodoGroupName(UpdateTodoGroupBody body);
        Task<ServiceResponse<TodoGroupBody>> DeleteTodoGroup(Guid id);
        Task<ServiceResponse<PaginationResponse<TodoGroupBody>>> GetTodoGroups(int limit, int offset, string token);
    }
}