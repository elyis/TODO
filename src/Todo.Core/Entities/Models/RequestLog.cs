namespace Todo.Core.Entities.Models
{
    public class RequestLog
    {
        public Guid Id { get; set; }
        public string IpAddress { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}