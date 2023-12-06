﻿using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Repositories;
using MySpot.Unit.Tests.Shared;
using Shouldly;
using Xunit;
using MySpot.Infrastructure.DAL.Repositories;

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
        public async Task Create_ForNotExistingDate_ShouldSucceed()
        {
            // Arrange
            var parkingSpots = await _weeklyParkingSpotRepository.GetAllAsync();
            var command = new CreateReservation(parkingSpots.First().Id,Guid.NewGuid(), _clock.Current().AddMinutes(5), "John Doe", "XYZ123");

            // Act
            var reservationId = await _reservationService.CreateAsync(command);

            // Assert
            reservationId.ShouldNotBeNull();
            reservationId.Value.ShouldBe(command.ReservationId);
        }

    }
}
