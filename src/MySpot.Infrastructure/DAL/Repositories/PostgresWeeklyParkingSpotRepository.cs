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

        public void AddReservation(WeeklyParkingSpot spot)
        {
            _dbContext.Add(spot);
            _dbContext.SaveChanges();
        }

        public void Delete(WeeklyParkingSpot spot)
        {
            _dbContext.Remove(spot);
            _dbContext.SaveChanges();
        }

        public WeeklyParkingSpot? Get(ParkingSpotId id)
            => _dbContext.WeeklyParkingSpots
                .Include(x => x.Reservations)//eager loading
                .SingleOrDefault(x => x.Id == id);

        public IEnumerable<WeeklyParkingSpot> GetAll()
            => _dbContext.WeeklyParkingSpots
                .Include(x => x.Reservations)//eager loading
                .ToList();

        public void Update(WeeklyParkingSpot spot)
        {
            _dbContext.Update(spot);
            _dbContext.SaveChanges();
        }
    }
}
