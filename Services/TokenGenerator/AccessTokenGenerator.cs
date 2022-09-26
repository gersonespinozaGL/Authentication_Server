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
        private readonly TokenGenerator _tokenGenerator;

        public AccessTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }
        public string generateToken(User user)
        {

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id",user.id.ToString()),
                new Claim(ClaimTypes.Email,user.email),
                new Claim(ClaimTypes.Name,user.username)
            };

            return _tokenGenerator.generateToken(
                _configuration.accessTokenSecret,
                _configuration.issuer,
                _configuration.audience,
                _configuration.accessTokenExpirationMinutes,
                claims
            );

        }
    }
}