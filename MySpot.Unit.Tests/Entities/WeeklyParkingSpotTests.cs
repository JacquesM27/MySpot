using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace MySpot.Unit.Tests.Entities
{
    public class WeeklyParkingSpotTests
    {
        #region Arrange
        private readonly WeeklyParkingSpot _spot;
        private readonly Date _now;
        public WeeklyParkingSpotTests()
        {
            _now = new Date(new DateTime(2023, 11, 29));
            _spot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_now), "P1");
        }
        #endregion

        [Theory]
        [InlineData("2023-11-28")]
        [InlineData("2023-12-08")]
        public void AddReservation_ForInvalidDate_ShouldFail(string dateString)
        {
            // Arrange
            var invalidDate = DateTime.Parse(dateString);
            var reservation = new Reservation(Guid.NewGuid(), _spot.Id, "John Doe", "XYZ123", new Date(invalidDate));

            // Act
            var exception = Record.Exception(() => _spot.AddReservation(reservation, _now));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidReservationDateException>();
        }

        [Fact]
        public void AddReservation_ForAlreadyExistingDate_ShouldFail()
        {
            // Arrange
            var reservation = new Reservation(Guid.NewGuid(), _spot.Id, "John Doe", "XYZ123", _now.AddDays(1));
            _spot.AddReservation(reservation, _now);
            var nextReservation = new Reservation(Guid.NewGuid(), _spot.Id, "John Doe", "XYZ123", _now.AddDays(1));

            // Act
            var exception = Record.Exception(() => _spot.AddReservation(nextReservation, _now));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ParkingSpotAlreadyReservedException>();
        }

        [Fact]
        public void AddReservation_ForNotExistingDate_ShouldSucceed()
        {
            // Arrange
            var reservation = new Reservation(Guid.NewGuid(), _spot.Id, "John Doe", "XYZ123", _now.AddDays(1));

            // Act
            _spot.AddReservation(reservation, _now);

            // Assert
            _spot.Reservations.ShouldHaveSingleItem();
        }
    }
}
