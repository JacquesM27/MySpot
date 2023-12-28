using MySpot.Application.Abstractions;
using MySpot.Application.DTO;

namespace MySpot.Application.Queries
{
    public sealed record GetUser(Guid UserId) : IQuery<UserDto?>;
}
