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
                                                             IParkingReservationService parkingReservationService,
                                                             IUserRepository userRepository)
        : ICommandHandler<ReserveParkingSpotForVehicle>
    {

        public async Task HandleAsync(ReserveParkingSpotForVehicle command)
        {
            var parkingSpotId = new ParkingSpotId(command.ParkingSpotId);
            var week = new Week(clock.Current());

            var weeklyParkingSpots = await repository.GetByWeekAsync(week);

            var parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId) 
                ?? throw new WeeklyParkingSpotNotFoundExeption(parkingSpotId);

            var user = await userRepository.GetByIdAsync(command.UserId)
                ?? throw new UserNotFoundException(command.UserId);

            var reservation = new VehicleReservation(command.ReservationId, user.Id, parkingSpotId, new EmployeeName(user.Fullname),
                command.LicensePlate, command.Capacity, new Date(command.Date));

            parkingReservationService.ReserveSpotForVehicle(weeklyParkingSpots, JobTitle.Boss, parkingSpotToReserve, reservation);
            await repository.UpdateAsync(parkingSpotToReserve);
        }
    }
}
