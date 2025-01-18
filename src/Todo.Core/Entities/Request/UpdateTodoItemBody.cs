using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Entities.Request
{
    public class UpdateTodoItemBody
    {
        [Required]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsDone { get; set; }
    }
}