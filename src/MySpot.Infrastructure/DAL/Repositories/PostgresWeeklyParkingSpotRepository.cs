using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories
{
    internal sealed class PostgresWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
    {
        private readonly MySpotDbContext _dbContext;

        public PostgresWeeklyParkingSpotRepository(MySpotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(WeeklyParkingSpot spot)
        {
            await _dbContext.AddAsync(spot);
        }

        public Task DeleteAsync(WeeklyParkingSpot spot)
        {
            _dbContext.Remove(spot);
            return Task.CompletedTask;
        }

        public Task<WeeklyParkingSpot?> GetAsync(ParkingSpotId id)
            => _dbContext.WeeklyParkingSpots
                .Include(x => x.Reservations)//eager loading
                .SingleOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync()
        {
            var result = await _dbContext.WeeklyParkingSpots
                .Include(x => x.Reservations)//eager loading
                .ToListAsync();
            return result;
        }
        public Task UpdateAsync(WeeklyParkingSpot spot)
        {
            _dbContext.Update(spot);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<WeeklyParkingSpot>> GetByWeekAsync(Week week)
            => await _dbContext.WeeklyParkingSpots
            .Include(x => x.Reservations)
            .Where(x => x.Week == week)
            .ToListAsync();
    }
}
