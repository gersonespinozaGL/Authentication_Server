namespace Models.Responses
{
    public class AuthenticatedUserResponse
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }
}