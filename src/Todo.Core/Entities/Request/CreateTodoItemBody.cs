using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Entities.Request
{
    public class CreateTodoItemBody
    {
        [StringLength(255, MinimumLength = 1), Required]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string? Description { get; set; }

        [Required]
        public bool IsDone { get; set; }

        [Required]
        public Guid TodoGroupId { get; set; }
    }
}