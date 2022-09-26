using Models;

namespace Services.TokenGenerator
{
    public class RefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _generator;

        public RefreshTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator generator)
        {
            _configuration = configuration;
            _generator = generator;
        }

        public string generateToken()
        {
            return _generator.generateToken(
               _configuration.refreshTokenSecret,
               _configuration.issuer,
               _configuration.audience,
               _configuration.refreshTokenExpirationMinutes
           );
        }
    }
}