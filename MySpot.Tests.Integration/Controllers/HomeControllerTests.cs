using Shouldly;
using Xunit;

namespace MySpot.Tests.Integration.Controllers
{
    public class HomeControllerTests : ControllerTestsBase
    {
        [Fact]
        public async Task GetBaseEndpoint_ForValidRequest_ShouldReturnOkStatusCode()
        {
            var response = await Client.GetAsync("/");
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldBe("MySpot API [test]");
        }
    }
}
