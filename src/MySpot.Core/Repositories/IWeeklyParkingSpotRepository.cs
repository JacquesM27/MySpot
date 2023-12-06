using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Repositories
{
    public interface IWeeklyParkingSpotRepository
    {
        Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync();
        Task<WeeklyParkingSpot?> GetAsync(ParkingSpotId id);
        Task AddAsync(WeeklyParkingSpot spot);
        Task UpdateAsync(WeeklyParkingSpot spot);
        Task DeleteAsync(WeeklyParkingSpot spot);
    }
}
