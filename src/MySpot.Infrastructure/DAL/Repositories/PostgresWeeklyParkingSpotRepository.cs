using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories
{
    internal sealed class PostgresWeeklyParkingSpotRepository(MySpotDbContext dbContext) 
        : IWeeklyParkingSpotRepository
    {
        private readonly DbSet<WeeklyParkingSpot> _weeklyParkingSpots = dbContext.WeeklyParkingSpots;

        public async Task AddAsync(WeeklyParkingSpot spot) 
            => await _weeklyParkingSpots.AddAsync(spot);

        public Task DeleteAsync(WeeklyParkingSpot spot)
        {
            _weeklyParkingSpots.Remove(spot);
            return Task.CompletedTask;
        }

        public Task<WeeklyParkingSpot?> GetAsync(ParkingSpotId id)
            => _weeklyParkingSpots
                .Include(x => x.Reservations)//eager loading
                .SingleOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync() 
            => await _weeklyParkingSpots
                .Include(x => x.Reservations)//eager loading
                .ToListAsync();

        public Task UpdateAsync(WeeklyParkingSpot spot)
        {
            _weeklyParkingSpots.Update(spot);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<WeeklyParkingSpot>> GetByWeekAsync(Week week)
            => await _weeklyParkingSpots
                .Include(x => x.Reservations)
                .Where(x => x.Week == week)
                .ToListAsync();
    }
}
