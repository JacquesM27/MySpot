using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly IClock _clock;
        private readonly IWeeklyParkingSpotRepository _repository;
        private readonly IParkingReservationService _parkingReservationService;

        public ReservationsService(IClock clock, IWeeklyParkingSpotRepository repository, IParkingReservationService parkingReservationService)
        {
            _clock = clock;
            _repository = repository;
            _parkingReservationService = parkingReservationService;
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
            var parkingSpotId = new ParkingSpotId(command.ParkingSpotId);
            var week = new Week(_clock.Current());
            var weeklyParkingSpots = await _repository.GetByWeekAsync(week);

            var parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId);
            if (parkingSpotToReserve is null)
                return default;

            var reservation = new Reservation(command.ReservationId, parkingSpotId, command.EmployeeName, command.LicensePlate, new Date(command.Date));

            _parkingReservationService.ReserveSpotForVehicle(weeklyParkingSpots, JobTitle.Employee, parkingSpotToReserve, reservation);
            //weeklyParkingSpot.AddReservation(reservation, new Date(_clock.Current()));
            await _repository.UpdateAsync(parkingSpotToReserve);

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
