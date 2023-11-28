using MySpot.Api.Entities;

namespace MySpot.Api.Services
{
    public class ReservationsService
    {
        private static int _id = 1;
        private static readonly List<Reservation> _reservations = [];
        private static readonly List<string> _parkingSpotNames = ["P1", "P2", "P3", "P4", "P5"];

        public Reservation? Get(int id) 
            => _reservations.SingleOrDefault(x => x.Id == id);

        public IEnumerable<Reservation> GetAll()
            => _reservations;

        public int? Create(Reservation reservation)
        {
            var now = DateTime.UtcNow.Date;
            var pastDays = now.DayOfWeek is DayOfWeek.Sunday ? 7 : (int)now.DayOfWeek;
            var remainingDays = 7 - pastDays;

            if (!_parkingSpotNames.Contains(reservation.ParkingSpotName))
                return default;

            if (!(reservation.Date.Date >= now && reservation.Date.Date <= now.AddDays(remainingDays)))
                return default;

            if (string.IsNullOrWhiteSpace(reservation.LicensePlate))
                return default;

            var reservationAlreadyExists = _reservations.Any(x =>
            x.ParkingSpotName == reservation.ParkingSpotName &&
            x.Date == reservation.Date);

            if (reservationAlreadyExists)
                return default;

            reservation.Id = _id;
            _id++;
            _reservations.Add(reservation);

            return _id;
        }

        public bool Update(Reservation reservation)
        {
            var existingReservation = _reservations.SingleOrDefault(x => x.Id == reservation.Id);
            if (existingReservation is null)
                return false;

            if (existingReservation.Date <= DateTime.UtcNow)
                return false;

            if (string.IsNullOrWhiteSpace(reservation.LicensePlate))
                return default;

            existingReservation.LicensePlate = reservation.LicensePlate;
            return true;
        }

        public bool Delete(int id)
        {
            var existingReservation = _reservations.SingleOrDefault(x => x.Id == id);
            if (existingReservation is null)
                return false;

            _reservations.Remove(existingReservation);
            return true;
        }
    }
}
