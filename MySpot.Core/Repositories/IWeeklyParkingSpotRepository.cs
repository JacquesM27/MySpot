using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Repositories
{
    public interface IWeeklyParkingSpotRepository
    {
        IEnumerable<WeeklyParkingSpot> GetAll();
        WeeklyParkingSpot? Get(ParkingSpotId id);
        void AddReservation(WeeklyParkingSpot spot);
        void Update(WeeklyParkingSpot spot);
        void Delete(WeeklyParkingSpot spot);
    }
}
