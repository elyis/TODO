namespace Todo.Core.Entities.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }

        public string? Token { get; set; }
        public DateTime? TokenValidBefore { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<TodoGroup> TodoGroups { get; set; } = new();
    }
}