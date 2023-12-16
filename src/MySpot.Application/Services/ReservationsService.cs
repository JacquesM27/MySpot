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
                    Employeename = x is VehicleReservation vehicleRes ? vehicleRes.EmployeeName : string.Empty,
                    Date = x.Date.Value.Date
                });
        }

        public async Task<Guid?> ReserveForVehicleAsync(ReserveParkingSpotForVehicle command)
        {
            var parkingSpotId = new ParkingSpotId(command.ParkingSpotId);
            var week = new Week(_clock.Current());

            var weeklyParkingSpots = await _repository.GetByWeekAsync(week);

            var parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId);
            if (parkingSpotToReserve is null)
                return default;

            var reservation = new VehicleReservation(command.ReservationId, parkingSpotId, command.EmployeeName, command.LicensePlate, new Date(command.Date));

            _parkingReservationService.ReserveSpotForVehicle(weeklyParkingSpots, JobTitle.Employee, parkingSpotToReserve, reservation);
            await _repository.UpdateAsync(parkingSpotToReserve);

            return reservation.Id;
        }

        public async Task ReserveForCleaningAsync(ReserveParkingSpotForCleaning command)
        {
            var week = new Week(command.Date);
            var weeklyParkingSpots = (await _repository.GetByWeekAsync(week)).ToList();

            _parkingReservationService.ReserveParkingForCleaning(weeklyParkingSpots, new Date(command.Date));

            //var tasks = weeklyParkingSpots.Select(x => _repository.UpdateAsync(x));
            //await Task.WhenAll(tasks);

            foreach (var parkingSpot in weeklyParkingSpots)
            {
                await _repository.UpdateAsync(parkingSpot);
            }
        }

        public async Task<bool> ChangeReservationLicensePlateAsync(ChangeReservationLicensePlate command)
        {
            var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);
            if (weeklyParkingSpot is null)
                return false;

            var existingReservation = weeklyParkingSpot.Reservations
                .OfType<VehicleReservation>()
                .SingleOrDefault(x => x.Id.Value == command.ReservationId);
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
