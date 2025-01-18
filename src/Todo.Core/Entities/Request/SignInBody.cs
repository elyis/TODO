using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Entities.Request
{
    public class SignInBody
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}