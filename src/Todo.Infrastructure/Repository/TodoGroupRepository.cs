using Microsoft.EntityFrameworkCore;
using Todo.Core.Entities.Models;
using Todo.Core.Entities.Request;
using Todo.Core.IRepository;
using Todo.Infrastructure.Data;

namespace Todo.Infrastructure.Repository
{
    public class TodoGroupRepository : ITodoGroupRepository
    {
        private readonly TodoDbContext _context;

        public TodoGroupRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<TodoGroup?> AddAsync(CreateTodoGroupBody body, Account account)
        {
            var todoGroup = new TodoGroup
            {
                Name = body.Name,
                Account = account
            };

            await _context.TodoGroups.AddAsync(todoGroup);
            await _context.SaveChangesAsync();
            return todoGroup;
        }

        public async Task<TodoGroup?> GetAsync(Guid id)
        {
            return await _context.TodoGroups.FindAsync(id);
        }

        public async Task<IEnumerable<TodoGroup>> GetAllByAccountIdAsync(int limit, int offset, Guid accountId)
        {
            return await _context.TodoGroups.Where(tg => tg.AccountId == accountId)
                                            .Skip(offset)
                                            .Take(limit)
                                            .ToListAsync();
        }

        public async Task<int> GetTotalCountByAccountIdAsync(Guid accountId)
        {
            return await _context.TodoGroups.Where(tg => tg.AccountId == accountId)
                .CountAsync();
        }

        public async Task<TodoGroup?> UpdateNameAsync(Guid id, string name)
        {
            var todoGroup = await _context.TodoGroups.FindAsync(id);
            if (todoGroup == null)
                return null;

            todoGroup.Name = name;
            await _context.SaveChangesAsync();
            return todoGroup;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var todoGroup = await _context.TodoGroups.FindAsync(id);
            if (todoGroup != null)
            {
                _context.TodoGroups.Remove(todoGroup);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}