using Todo.Core.Entities.Response;

namespace Todo.Core.Entities.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public TodoGroup TodoGroup { get; set; }
        public Guid TodoGroupId { get; set; }

        public TodoItemBody ToTodoItemBody()
        {
            return new TodoItemBody
            {
                Id = Id,
                Name = Name,
                Description = Description,
                IsDone = IsDone,
                CreatedAt = CreatedAt
            };
        }
    }
}