using Todo.Core.Entities.Models;
using Todo.Core.Entities.Request;

namespace Todo.Core.IRepository
{
    public interface ITodoGroupRepository
    {
        Task<TodoGroup?> AddAsync(CreateTodoGroupBody body, Account account);
        Task<TodoGroup?> GetAsync(Guid id);
        Task<IEnumerable<TodoGroup>> GetAllByAccountIdAsync(int limit, int offset, Guid accountId);
        Task<TodoGroup?> UpdateNameAsync(Guid id, string name);
        Task<bool> DeleteAsync(Guid id);
        Task<int> GetTotalCountByAccountIdAsync(Guid accountId);
    }
}