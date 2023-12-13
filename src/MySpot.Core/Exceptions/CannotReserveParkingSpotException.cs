using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions
{
    public sealed class CannotReserveParkingSpotException(ParkingSpotId parkingSpotId)
        : CustomException($"Cannot reserve parking spot with ID: {parkingSpotId}")
    {
        public ParkingSpotId ParkingSpotId { get; } = parkingSpotId;
    }
}
