using Todo.Core.Entities.Models;
using Todo.Core.Entities.Request;

namespace Todo.Core.IRepository
{
    public interface ITodoItemRepository
    {
        Task<TodoItem> CreateTodoItem(CreateTodoItemBody body, TodoGroup todoGroup);
        Task<TodoItem?> GetTodoItemById(Guid id);
        Task<TodoItem?> UpdateTodoItem(UpdateTodoItemBody body);
        Task<int> GetTotalCountByGroupIdAsync(Guid groupId);
        Task<bool> DeleteTodoItem(Guid id);
        Task<IEnumerable<TodoItem>> GetTodoItemsByGroupId(Guid groupId, int limit, int offset);
    }
}