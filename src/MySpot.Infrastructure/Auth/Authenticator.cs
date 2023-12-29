using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySpot.Application.DTO;
using MySpot.Application.Security;
using MySpot.Core.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MySpot.Infrastructure.Auth
{
    internal sealed class Authenticator(IClock clock,
                                        IOptions<AuthOptions> options)
        : IAuthenticator
    {
        private readonly string _issuer = options.Value.Issuer;
        private readonly string _audience = options.Value.Audience;
        private readonly TimeSpan _expiry = options.Value.Expiry ?? TimeSpan.FromHours(1);
        private readonly SigningCredentials _signingCredentials = new(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SigningKey)),
                SecurityAlgorithms.HmacSha512);
        private readonly JwtSecurityTokenHandler _jwtHandler = new();

        public JwtDto CreateToken(Guid userId, string role)
        {
            var now = clock.Current();
            var expires = now.Add(_expiry);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
                new(ClaimTypes.Role, role)
            };

            var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredentials);
            var accessToken = _jwtHandler.WriteToken(jwt);
            return new JwtDto { AccessToken = accessToken };
        }
    }
}
