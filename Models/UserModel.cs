using System.ComponentModel.DataAnnotations;

namespace MailArchive.WebAPI.Models
{
    public class UserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
