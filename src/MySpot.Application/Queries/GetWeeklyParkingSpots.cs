using MySpot.Application.Abstractions;
using MySpot.Application.DTO;

namespace MySpot.Application.Queries
{
    public sealed record GetWeeklyParkingSpots(DateTime? Date) 
        : IQuery<IEnumerable<WeeklyParkingSpotDto>>;
}
