using Microsoft.EntityFrameworkCore;
using MySpot.Application.Abstractions;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Handlers
{
    internal sealed class GetUserHandler(MySpotDbContext dbContext)
        : IQueryHandler<GetUser, UserDto?>
    {
        public async Task<UserDto?> HandleAsync(GetUser query)
        {
            var userId = new UserId(query.UserId);
            var user = await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId);

            return user?.AsDto();
        }
    }
}
