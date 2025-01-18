using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Entities.Request
{
    public class CreateTodoGroupBody
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }
    }
}