using MySpot.Application.Commands;
using MySpot.Core.ValueObjects;
using Shouldly;
using System.Net.Http.Json;
using Xunit;

namespace MySpot.Tests.Integration.Controllers
{
    public class UsersControllerTests : ControllerTestsBase
    {

        [Fact]
        public async Task PostUsers_GivenValidDate_ShouldReturnCreatedStatusCode()
        {
            // Arrange
            var command = new SignUp(Guid.Empty, "test-user1@myspot.io", "test-user1", "secret", "John Doe", Role.User());

            // Act
            var response = await Client.PostAsJsonAsync("users", command);

            // Assert
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);
            //location header should contain data from CreatedAtAction(nameof(Get), new {command.UserId}, null);
            response.Headers.Location.ShouldNotBeNull();
        }
    }
}
