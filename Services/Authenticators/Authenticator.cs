using System.Threading.Tasks;
using Models;
using Models.Responses;
using Services.RefreshTokenRepositories;
using Services.TokenGenerator;

namespace Services.Authenticators
{
    public class Authenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public Authenticator(AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator, IRefreshTokenRepository refreshTokenRepository)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticatedUserResponse> authenticate(User user)
        {

            string accessToken = _accessTokenGenerator.generateToken(user);
            string refreshToken = _refreshTokenGenerator.generateToken();

            RefreshToken refreshTokenDTO = new RefreshToken()
            {
                token = refreshToken,
                userId = user.id
            };
            await _refreshTokenRepository.create(refreshTokenDTO);
            return new AuthenticatedUserResponse()
            {
                accessToken = accessToken,
                refreshToken = refreshToken
            };
        }
    }
}