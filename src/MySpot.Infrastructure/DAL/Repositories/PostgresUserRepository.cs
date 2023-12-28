﻿using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories
{
    internal sealed class PostgresUserRepository(MySpotDbContext dbContext)
                : IUserRepository
    {
        private readonly DbSet<User> _users = dbContext.Users;

        public async Task AddAsync(User user)
            => await _users.AddAsync(user);

        public Task<User?> GetByIdAsync(UserId id)
            => _users.SingleOrDefaultAsync(x => x.Id == id);

        public Task<User?> GetByUsernameAsync(Username username)
            => _users.SingleOrDefaultAsync(x => x.Username == username);

        public Task<User?> GetByEmailAsync(Email email)
            => _users.SingleOrDefaultAsync(x => x.Email == email);
    }
}
