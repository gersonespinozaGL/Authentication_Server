using System;

namespace Models
{
    public class RefreshToken
    {
        public Guid id { get; set; }
        public string token { get; set; }
        public Guid userId { get; set; }
    }
}