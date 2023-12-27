using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;

namespace MySpot.Application.Commands.Handlers
{
    internal sealed class ChangeReservationLicensePlateHandler(IWeeklyParkingSpotRepository Repository)
        : ICommandHandler<ChangeReservationLicensePlate>
    {
        public async Task HandleAsync(ChangeReservationLicensePlate command)
        {
            var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId) 
                ?? throw new WeeklyParkingSpotNotFoundExeption(command.ReservationId);

            var existingReservation = weeklyParkingSpot.Reservations
                .OfType<VehicleReservation>()
                .SingleOrDefault(x => x.Id.Value == command.ReservationId)
                ?? throw new ReservationNotFoundException(command.ReservationId);

            existingReservation.ChangeLicensePlate(command.LicensePlate);
            await Repository.UpdateAsync(weeklyParkingSpot);
        }

        private async Task<WeeklyParkingSpot?> GetWeeklyParkingSpotByReservationAsync(Guid reservationId)
        {
            var spots = await Repository.GetAllAsync();
            return spots.SingleOrDefault(x => x.Reservations.Any(r => r.Id.Value == reservationId));
        }
    }
}
