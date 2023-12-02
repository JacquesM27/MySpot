using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects
{
    public sealed record ParkingSpotName(string Value)
    {
        public string Value { get; } = Value ?? throw new InvalidParkingSpotNameException();

        public static implicit operator ParkingSpotName(string Value) => new(Value);

        public static implicit operator string(ParkingSpotName Value) => Value.Value;
    }
}
