using Microsoft.EntityFrameworkCore;
using Todo.Core.Entities.Models;
using Todo.Core.Entities.Request;
using Todo.Core.IRepository;
using Todo.Infrastructure.Data;

namespace Todo.Infrastructure.Repository
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly TodoDbContext _context;

        public TodoItemRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem> CreateTodoItem(CreateTodoItemBody body, TodoGroup todoGroup)
        {
            var todoItem = new TodoItem
            {
                Name = body.Name,
                Description = body.Description,
                IsDone = body.IsDone,
                TodoGroup = todoGroup
            };

            await _context.TodoItems.AddAsync(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        public async Task<TodoItem?> GetTodoItemById(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<TodoItem?> UpdateTodoItem(UpdateTodoItemBody body)
        {
            var todoItem = await GetTodoItemById(body.Id);
            if (todoItem == null) return null;

            if (body.Name != null)
                todoItem.Name = body.Name;

            if (body.Description != null)
                todoItem.Description = body.Description;

            if (body.IsDone != null)
                todoItem.IsDone = body.IsDone.Value;

            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsByGroupId(Guid groupId, int limit, int offset)
        {
            return await _context.TodoItems.Where(item => item.TodoGroupId == groupId)
                                           .OrderByDescending(item => item.CreatedAt)
                                           .Skip(offset)
                                           .Take(limit)
                                           .ToListAsync();
        }

        public async Task<int> GetTotalCountByGroupIdAsync(Guid groupId)
        {
            return await _context.TodoItems.Where(item => item.TodoGroupId == groupId)
                                           .CountAsync();
        }

        public async Task<bool> DeleteTodoItem(Guid id)
        {
            var todoItem = await GetTodoItemById(id);
            if (todoItem != null)
            {
                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}