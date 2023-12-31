using Microsoft.Extensions.Options;
using MySpot.Application.DTO;
using MySpot.Application.Security;
using MySpot.Infrastructure.Auth;
using MySpot.Infrastructure.Time;
using System.Net.Http.Headers;
using Xunit;

namespace MySpot.Tests.Integration.Controllers
{
    [Collection("api")]
    public abstract class ControllerTestsBase : IClassFixture<OptionsProvider>
    {
        private readonly IAuthenticator _authenticator;
        internal MySpotTestApp _app;
        protected HttpClient Client { get; }

        public ControllerTestsBase(OptionsProvider optionsProvider)
        {
            var authOptions = optionsProvider.Get<AuthOptions>("auth");
            _authenticator = new Authenticator(new Clock(), new OptionsWrapper<AuthOptions>(authOptions));

            _app = new MySpotTestApp();
            Client = _app.Client;
        }

        protected JwtDto Authorize(Guid userId, string role)
        {
            var jwt = _authenticator.CreateToken(userId, role);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.AccessToken);
            return jwt;
        }
    }
}
