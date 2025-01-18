namespace Todo.Core.Entities.Response
{
    public class TodoItemBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}