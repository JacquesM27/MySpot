using MySpot.Application.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers
{
    internal class ReserveParkingSpotForCleaningHandler(IWeeklyParkingSpotRepository Repository,
                                                        IParkingReservationService ParkingReservationService)
        : ICommandHandler<ReserveParkingSpotForCleaning>
    {
        public async Task HandleAsync(ReserveParkingSpotForCleaning command)
        {
            var week = new Week(command.Date);
            var weeklyParkingSpots = (await Repository.GetByWeekAsync(week)).ToList();

            ParkingReservationService.ReserveParkingForCleaning(weeklyParkingSpots, new Date(command.Date));

            //var tasks = weeklyParkingSpots.Select(Repository.UpdateAsync);
            //await Task.WhenAll(tasks);

            foreach (var parkingSpot in weeklyParkingSpots)
            {
                await Repository.UpdateAsync(parkingSpot);
            }
        }
    }
}
