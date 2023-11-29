using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Services
{
    public class ReservationsService
    {
        private static readonly Clock _clock = new();
        private static readonly List<WeeklyParkingSpot> WeeklyParkingSpots =
        [
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(_clock.Current()), "P1"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(_clock.Current()), "P2"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(_clock.Current()), "P3"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(_clock.Current()), "P4"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(_clock.Current()), "P5"),
        ];

        public ReservationDto? Get(Guid id)  
            => GetAllWeekly().SingleOrDefault(x => x.Id == id);

        public IEnumerable<ReservationDto> GetAllWeekly()
            => WeeklyParkingSpots.SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto
            {
                Id = x.Id,
                ParkingSpotId = x.ParkingSpotId,
                Employeename = x.EmployeeName,
                Date = x.Date.Value.Date
            });

        public Guid? Create(CreateReservation command)
        {
            var weeklyParkingSpot = WeeklyParkingSpots.SingleOrDefault(x => x.Id.Value == command.ParkingSpotId);
            if (weeklyParkingSpot is null)
            {
                return default;
            }

            var reservation = new Reservation(command.ReservationId, command.ParkingSpotId, command.EmployeeName, command.LicensePlate, new Date(command.Date));
            weeklyParkingSpot.AddReservation(reservation, new Date(_clock.Current()));

            return reservation.Id;
        }

        public bool Update(ChangeReservationLicensePlate command)
        {
            var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
            if (weeklyParkingSpot is null)
                return false;

            var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id.Value == command.ReservationId);
            if (existingReservation is null)
                return false;

            if (existingReservation.Date <= new Date(_clock.Current()))
                return false;

            existingReservation.ChangeLicensePlate(command.LicensePlate);
            return true;
        }

        public bool Delete(DeleteReservation command)
        {
            var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
            if (weeklyParkingSpot is null)
                return false;

            var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id.Value == command.ReservationId);
            if (existingReservation is null)
                return false;

            weeklyParkingSpot.RemoveReservation(command.ReservationId);
            return true;
        }

        private static WeeklyParkingSpot? GetWeeklyParkingSpotByReservation(Guid reservationId)
            => WeeklyParkingSpots.SingleOrDefault(x => x.Reservations.Any(r => r.Id.Value == reservationId));
    }
}
