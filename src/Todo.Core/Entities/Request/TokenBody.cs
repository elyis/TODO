using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Entities.Request
{
    public class TokenBody
    {
        [Required]
        public string Token { get; set; }
    }
}