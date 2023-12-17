using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions
{
    public sealed class ParkingSpotCapacityExceededException(ParkingSpotId parkingSpotId) 
        : CustomException($"Parking spot with ID: {parkingSpotId} exceeds its reservation capacity.")
    {
        public ParkingSpotId ParkingSpotId => parkingSpotId;
    }
}
