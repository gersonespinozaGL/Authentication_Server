using System.ComponentModel.DataAnnotations;

namespace Models.Requests
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string confirmPassword { get; set; }

    }
}