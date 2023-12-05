using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities
{
    public class WeeklyParkingSpot
    {
        private readonly HashSet<Reservation> _reservations = [];

        public ParkingSpotId Id { get; }
        public Week Week { get; }
        public string Name { get; }
        public IEnumerable<Reservation> Reservations => _reservations;

        public WeeklyParkingSpot(ParkingSpotId id, Week week, string name)
        {
            Id = id;
            Week = week;
            Name = name;
        }

        private WeeklyParkingSpot() { }

        public void AddReservation(Reservation reservation, Date now)
        {
            var isInvalidDate = reservation.Date < Week.From ||
                                reservation.Date > Week.To ||
                                reservation.Date < now;

            if (isInvalidDate)
                throw new InvalidReservationDateException(reservation.Date.Value.Date);

            var reservationAlreadyExists = _reservations.Any(x =>
              x.Date == reservation.Date);

            if (reservationAlreadyExists)
                throw new ParkingSpotAlreadyReservedException(Name, reservation.Date.Value.Date);

            _reservations.Add(reservation);
        }

        public void RemoveReservation(ReservationId reservationId)
        {
            var item = _reservations.SingleOrDefault(x => x.Id == reservationId);
            if (item is null)
                return;

            _reservations.Remove(item);
        }
    }
}
