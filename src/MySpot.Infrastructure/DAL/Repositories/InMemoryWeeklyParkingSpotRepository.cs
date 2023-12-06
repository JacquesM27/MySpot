using MySpot.Application.Services;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories
{
    internal class InMemoryWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
    {
        private readonly IClock _clock;
        private readonly List<WeeklyParkingSpot> _parkingSpots;

        public InMemoryWeeklyParkingSpotRepository(IClock clock)
        {
            _clock = clock;
            _parkingSpots = [
            new(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(_clock.Current()), "P1"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(_clock.Current()), "P2"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(_clock.Current()), "P3"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(_clock.Current()), "P4"),
                new(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(_clock.Current()), "P5"),
            ];
        }

        public Task AddAsync(WeeklyParkingSpot spot)
        {
            _parkingSpots.Add(spot);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(WeeklyParkingSpot spot)
        {
            _parkingSpots.Remove(spot);
            return Task.CompletedTask;
        }

        public Task<WeeklyParkingSpot?> GetAsync(ParkingSpotId id)
            => Task.FromResult(_parkingSpots.SingleOrDefault(x => x.Id == id));

        public Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync()
            => Task.FromResult(_parkingSpots.AsEnumerable());

        public Task UpdateAsync(WeeklyParkingSpot spot) 
            => Task.CompletedTask;
    }
}
