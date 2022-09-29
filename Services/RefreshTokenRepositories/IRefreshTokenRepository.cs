using System.Threading.Tasks;
using Models;

namespace Services.RefreshTokenRepositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> getByToken(string token);
        Task create(RefreshToken refreshToken);
    }
}