using System.ComponentModel.DataAnnotations;

namespace Afsw.Command.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(64, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
