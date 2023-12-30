using Shouldly;
using Xunit;

namespace MySpot.Tests.Integration.Controllers
{
    public class HomeControllerTests
    {
        private readonly MySpotTestApp _app;

        public HomeControllerTests()
        {
            _app = new MySpotTestApp();
        }

        [Fact]
        public async Task GetBaseEndpoint_ForValidRequest_ShouldReturnOkStatusCode()
        {
            var response = await _app.Client.GetAsync("/");
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldBe("MySpot API [test]");
        }
    }
}
