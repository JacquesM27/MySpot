using MySpot.Application.Time;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.Repositories
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

        public void AddReservation(WeeklyParkingSpot spot)
            => _parkingSpots.Add(spot);

        public void Delete(WeeklyParkingSpot spot)
            => _parkingSpots.Remove(spot);

        public WeeklyParkingSpot? Get(ParkingSpotId id)
            => _parkingSpots.SingleOrDefault(x => x.Id == id);

        public IEnumerable<WeeklyParkingSpot> GetAll()
            => _parkingSpots;

        public void Update(WeeklyParkingSpot spot) { }
    }
}
