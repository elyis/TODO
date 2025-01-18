using Todo.Core.Entities.Response;

namespace Todo.Core.Entities.Models
{
    public class TodoGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Account Account { get; set; }
        public Guid AccountId { get; set; }
        public List<TodoItem> TodoItems { get; set; } = new();

        public TodoGroupBody ToTodoGroupBody()
        {
            return new TodoGroupBody
            {
                Id = Id,
                Name = Name,
            };
        }
    }
}