using MySpot.Application.Abstractions;
using MySpot.Application.DTO;

namespace MySpot.Application.Queries
{
    public sealed record GetUsers() : IQuery<IEnumerable<UserDto>>;
}
