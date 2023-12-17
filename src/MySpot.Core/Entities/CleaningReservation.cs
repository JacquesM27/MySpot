using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities
{
    public sealed class CleaningReservation : Reservation
    {
        public CleaningReservation(ReservationId id, ParkingSpotId parkingSpotId, Date date) 
            : base(id, parkingSpotId, 2, date)
        {
        }

        private CleaningReservation() { }
    }
}
