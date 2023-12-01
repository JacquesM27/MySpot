using MySpot.Api.Commands;
using MySpot.Api.Entities;
using MySpot.Api.Repositories;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;
using MySpot.Unit.Tests.Shared;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MySpot.Unit.Tests.Services
{
    public class ReservationServiceTests
    {
        #region Arrange
        private readonly IClock _clock;
        private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
        private readonly IReservationsService _reservationService;

        public ReservationServiceTests()
        {
            _clock = new TestClock();
            _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
            _reservationService = new ReservationsService(_clock, _weeklyParkingSpotRepository);
        }
        #endregion

        [Fact]
        public void Create_ForNotExistingDate_ShouldSucceed()
        {
            // Arrange
            var command = new CreateReservation(_weeklyParkingSpotRepository.GetAll().First().Id,Guid.NewGuid(), _clock.Current().AddMinutes(5), "John Doe", "XYZ123");

            // Act
            var reservationId = _reservationService.Create(command);

            // Assert
            reservationId.ShouldNotBeNull();
            reservationId.Value.ShouldBe(command.ReservationId);
        }

    }
}
