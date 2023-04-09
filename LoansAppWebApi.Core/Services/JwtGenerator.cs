using LoansAppWebApi.Core.Constants;
using LoansAppWebApi.Core.Interfaces;
using LoansAppWebApi.Models.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Core.Services
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly JWTConfiguration _jwtConfiguration;

        public JwtGenerator(IOptions<JWTConfiguration> options)
        {
            this._jwtConfiguration = options.Value;
        }

        public string GenerateToken(string userId)
        {
            var authSigngingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.AccessTokenSecret));
            var tokenHandler = new JwtSecurityTokenHandler();

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, AuthConstants.UserRoles.User),
                new Claim(ClaimTypes.Email, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                expires: DateTime.Now.AddMinutes(_jwtConfiguration.AccessTokenExpirationMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigngingKey, SecurityAlgorithms.HmacSha256)
                );

            return tokenHandler.WriteToken(token);
        }
    }
}
