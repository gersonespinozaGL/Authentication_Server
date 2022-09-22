namespace Models
{
    public class AuthenticationConfiguration
    {
        public string accessToken { get; set; }
        public int accessTokenExpirationMinutes { get; set; }
        public string issuer { get; set; }
        public string audience { get; set; }
    }
}