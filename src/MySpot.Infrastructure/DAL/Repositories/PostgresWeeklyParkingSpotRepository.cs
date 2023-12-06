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
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(WeeklyParkingSpot spot)
        {
            _dbContext.Remove(spot);
            await _dbContext.SaveChangesAsync();
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
        public async Task UpdateAsync(WeeklyParkingSpot spot)
        {
            _dbContext.Update(spot);
            await _dbContext.SaveChangesAsync();
        }
    }
}
