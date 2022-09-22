using System;

namespace Models
{
    public class User
    {
        public Guid id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string passwordHash { get; set; }
        
    }
}