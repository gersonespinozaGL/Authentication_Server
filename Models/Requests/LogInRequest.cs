using System.ComponentModel.DataAnnotations;

namespace Models.Requests
{
    public class LogInRequest
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}