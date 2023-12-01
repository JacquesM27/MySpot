using MySpot.Api.Commands;
using MySpot.Api.Entities;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;
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
        private static readonly Clock _clock = new();
        private readonly List<WeeklyParkingSpot> WeeklyParkingSpots =
        [
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(_clock.Current()), "P1"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(_clock.Current()), "P2"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(_clock.Current()), "P3"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(_clock.Current()), "P4"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(_clock.Current()), "P5"),
        ];
        public readonly ReservationsService _reservationService;

        public ReservationServiceTests()
        {
            _reservationService = new ReservationsService(WeeklyParkingSpots);
        }
        #endregion

        [Fact]
        public void Create_ForNotExistingDate_ShouldSucceed()
        {
            // Arrange
            var command = new CreateReservation(WeeklyParkingSpots.First().Id,Guid.NewGuid(), DateTime.UtcNow.AddMinutes(5), "John Doe", "XYZ123");

            // Act
            var reservationId = _reservationService.Create(command);

            // Assert
            reservationId.ShouldNotBeNull();
            reservationId.Value.ShouldBe(command.ReservationId);
        }

    }
}
