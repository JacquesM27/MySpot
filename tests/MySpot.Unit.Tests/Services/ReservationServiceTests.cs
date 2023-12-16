using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Repositories;
using MySpot.Unit.Tests.Shared;
using Shouldly;
using Xunit;
using MySpot.Infrastructure.DAL.Repositories;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Policies;

namespace MySpot.Unit.Tests.Services
{
    public class ReservationServiceTests
    {
        #region Arrange
        private readonly IClock _clock;
        private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
        private readonly IReservationsService _reservationService;
        private readonly IParkingReservationService _parkingReservationService;

        public ReservationServiceTests()
        {
            _clock = new TestClock();
            _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);

            _parkingReservationService = new ParkingReservationService(new IReservationPolicy[]
            {
               new BossReservationPolicy(),
               new ManagerReservationPolicy(),
               new RegularEmployeeReservationPolicy(_clock)
            },_clock);

            _reservationService = new ReservationsService(_clock, _weeklyParkingSpotRepository, _parkingReservationService);
        }
        #endregion

        [Fact]
        public async Task Create_ForNotExistingDate_ShouldSucceed()
        {
            // Arrange
            var parkingSpots = await _weeklyParkingSpotRepository.GetAllAsync();
            var command = new ReserveParkingSpotForVehicle(parkingSpots.First().Id,Guid.NewGuid(), _clock.Current().AddMinutes(5), "John Doe", "XYZ123");

            // Act
            var reservationId = await _reservationService.ReserveForVehicleAsync(command);

            // Assert
            reservationId.ShouldNotBeNull();
            reservationId.Value.ShouldBe(command.ReservationId);
        }

    }
}
