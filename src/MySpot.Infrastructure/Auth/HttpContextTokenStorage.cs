using Microsoft.AspNetCore.Http;
using MySpot.Application.DTO;
using MySpot.Application.Security;

namespace MySpot.Infrastructure.Auth
{
    internal sealed class HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor)
        : ITokenStorage
    {
        private const string TokenKey = "jwt";

        public JwtDto? Get()
        {
            if (httpContextAccessor is null)
                return null;

            if (httpContextAccessor.HttpContext!.Items.TryGetValue(TokenKey, out var jwt))
                return jwt as JwtDto;

            return null;
        }

        public void Set(JwtDto jwt)
            => httpContextAccessor.HttpContext?.Items.Add(TokenKey, jwt);
    }
}
