using Microsoft.AspNetCore.Identity;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Security;
using MySpot.Infrastructure.Time;
using Shouldly;
using System.Net.Http.Json;
using Xunit;

namespace MySpot.Tests.Integration.Controllers
{
    public class UsersControllerTests(OptionsProvider optionsProvider) : ControllerTestsBase(optionsProvider), IDisposable
    {
        private readonly TestDatabase _testDb = new();
        private const string Password = "secret";

        public void Dispose() => _testDb.Dispose();

        [Fact]
        public async Task PostSignUp_GivenValidDate_ShouldReturnCreatedStatusCode()
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

        [Fact]
        public async Task PostSignIn_ForExistingUser_ShouldReturnOkStatusCodeAndJwt()
        {
            // Arrange
            var user = await CreateUserAsync();
            var command = new SignIn(user.Email, Password);

            // Act
            var response = await Client.PostAsJsonAsync("users/sign-in", command);

            // Assert
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            var jwt = await response.Content.ReadFromJsonAsync<JwtDto>();
            jwt.ShouldNotBeNull();
            jwt.AccessToken.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task GetMe_ForAuthorizedUser_ShouldReturnOkStatusAndData()
        {
            // Arrange
            var user = await CreateUserAsync();
            Authorize(user.Id, user.Role);

            // Act
            var userDto = await Client.GetFromJsonAsync<UserDto>("users/me");

            // Assert
            userDto.ShouldNotBeNull();
            userDto.Id.ShouldBe(user.Id.Value);
        }

        private async Task<User> CreateUserAsync()
        {
            var clock = new Clock();
            var passwordManager = new PasswordManager(new PasswordHasher<User>());
            var user = new User(Guid.NewGuid(), "test-user1@myspot.io", "test-user1", passwordManager.Secure(Password), "John Doe", Role.User(), clock.Current());
            await _testDb.DbContext.Users.AddAsync(user);
            await _testDb.DbContext.SaveChangesAsync();
            return user;
        }
    }
}
