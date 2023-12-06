using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly IClock _clock;
        private readonly IWeeklyParkingSpotRepository _repository;

        public ReservationsService(IClock clock, IWeeklyParkingSpotRepository repository)
        {
            _clock = clock;
            _repository = repository;
        }

        public async Task<ReservationDto?> GetAsync(Guid id)
        {
            var reservations = await GetAllWeeklyAsync();
            return reservations.Where(x => x.Id == id).SingleOrDefault();
        }

        public async Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync()
        {
            var weeklyParkingSpots = await _repository.GetAllAsync();
            return weeklyParkingSpots.
                SelectMany(x => x.Reservations)
                .Select(x => new ReservationDto
                {
                    Id = x.Id,
                    ParkingSpotId = x.ParkingSpotId,
                    Employeename = x.EmployeeName,
                    Date = x.Date.Value.Date
                });
        }

        public async Task<Guid?> CreateAsync(CreateReservation command)
        {
            var weeklyParkingSpot = await _repository.GetAsync(command.ParkingSpotId);
            if (weeklyParkingSpot is null)
            {
                return default;
            }

            var reservation = new Reservation(command.ReservationId, command.ParkingSpotId, command.EmployeeName, command.LicensePlate, new Date(command.Date));
            weeklyParkingSpot.AddReservation(reservation, new Date(_clock.Current()));
            await _repository.UpdateAsync(weeklyParkingSpot);

            return reservation.Id;
        }

        public async Task<bool> UpdateAsync(ChangeReservationLicensePlate command)
        {
            var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);
            if (weeklyParkingSpot is null)
                return false;

            var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id.Value == command.ReservationId);
            if (existingReservation is null)
                return false;

            if (existingReservation.Date <= new Date(_clock.Current()))
                return false;

            existingReservation.ChangeLicensePlate(command.LicensePlate);
            await _repository.UpdateAsync(weeklyParkingSpot);
            return true;
        }

        public async Task<bool> DeleteAsync(DeleteReservation command)
        {
            var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);
            if (weeklyParkingSpot is null)
                return false;

            var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id.Value == command.ReservationId);
            if (existingReservation is null)
                return false;

            weeklyParkingSpot.RemoveReservation(command.ReservationId);
            await _repository.DeleteAsync(weeklyParkingSpot);
            return true;
        }

        private async Task<WeeklyParkingSpot?> GetWeeklyParkingSpotByReservationAsync(Guid reservationId)
        {
            var spots = await _repository.GetAllAsync();
            return spots.SingleOrDefault(x => x.Reservations.Any(r => r.Id.Value == reservationId));
        }
    }
}
