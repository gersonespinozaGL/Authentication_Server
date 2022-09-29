using System.ComponentModel.DataAnnotations;

namespace Models.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string refreshToken { get; set; }
    }
}