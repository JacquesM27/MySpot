using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Tests.Integration
{
    internal class TestUserRepository : IUserRepository
    {
        private readonly List<User> users = [];

        public Task AddAsync(User user)
        {
            users.Add(user);
            return Task.CompletedTask;
        }

        public Task<User?> GetByEmailAsync(Email email)
            => Task.FromResult(users.FirstOrDefault(x => x.Email == email));

        public Task<User?> GetByIdAsync(UserId id)
            => Task.FromResult(users.FirstOrDefault(x => x.Id == id));

        public Task<User?> GetByUsernameAsync(Username username)
            => Task.FromResult(users.FirstOrDefault(x => x.Username == username));
    }
}
