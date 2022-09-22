using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Services.TokenGenerator
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;

        public AccessTokenGenerator(AuthenticationConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Generate(User user)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.accessToken));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id",user.id.ToString()),
                new Claim(ClaimTypes.Email,user.email),
                new Claim(ClaimTypes.Name,user.username)
            };

            JwtSecurityToken token = new JwtSecurityToken(
                _configuration.issuer,
                _configuration.audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(_configuration.accessTokenExpirationMinutes),
                credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}