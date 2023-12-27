using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers
{
    internal sealed class ReserveParkingSpotForVehicleHandler(IClock clock,
                                                             IWeeklyParkingSpotRepository repository,
                                                             IParkingReservationService parkingReservationService)
        : ICommandHandler<ReserveParkingSpotForVehicle>
    {

        public async Task HandleAsync(ReserveParkingSpotForVehicle command)
        {
            var parkingSpotId = new ParkingSpotId(command.ParkingSpotId);
            var week = new Week(clock.Current());

            var weeklyParkingSpots = await repository.GetByWeekAsync(week);

            var parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId) 
                ?? throw new WeeklyParkingSpotNotFoundExeption(parkingSpotId);

            var reservation = new VehicleReservation(command.ReservationId, parkingSpotId, command.EmployeeName,
                command.LicensePlate, command.Capacity, new Date(command.Date));

            parkingReservationService.ReserveSpotForVehicle(weeklyParkingSpots, JobTitle.Boss, parkingSpotToReserve, reservation);
            await repository.UpdateAsync(parkingSpotToReserve);
        }
    }
}
