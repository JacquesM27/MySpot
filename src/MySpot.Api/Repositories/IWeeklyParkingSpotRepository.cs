using MySpot.Api.Entities;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Repositories
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
