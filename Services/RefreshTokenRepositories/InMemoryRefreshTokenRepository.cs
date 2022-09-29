using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using System.Linq;

namespace Services.RefreshTokenRepositories
{
    public class InMemoryRefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        public Task create(RefreshToken refreshToken)
        {
            refreshToken.id = Guid.NewGuid();
            _refreshTokens.Add(refreshToken);
            return Task.CompletedTask;
        }

        public Task<RefreshToken> getByToken(string token)
        {
            RefreshToken refreshToken = _refreshTokens.FirstOrDefault(t => t.token == token);
            return Task.FromResult(refreshToken);
        }
    }
}