using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;

namespace MySpot.Application.Commands.Handlers
{
    internal sealed class DeleteReservationHandler(IWeeklyParkingSpotRepository Repository)
        : ICommandHandler<DeleteReservation>
    {
        public async Task HandleAsync(DeleteReservation command)
        {
            var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId)
                ?? throw new WeeklyParkingSpotNotFoundExeption(command.ReservationId);

            var existingReservation = weeklyParkingSpot.Reservations
                .SingleOrDefault(x => x.Id.Value == command.ReservationId)
                ?? throw new WeeklyParkingSpotNotFoundExeption(command.ReservationId);

            weeklyParkingSpot.RemoveReservation(command.ReservationId);
            await Repository.DeleteAsync(weeklyParkingSpot);
        }

        private async Task<WeeklyParkingSpot?> GetWeeklyParkingSpotByReservationAsync(Guid reservationId)
        {
            var spots = await Repository.GetAllAsync();
            return spots.SingleOrDefault(x => x.Reservations.Any(r => r.Id.Value == reservationId));
        }
    }
}
