namespace MySpot.Core.Exceptions
{
    public sealed class ParkingSpotAlreadyReservedException(string name, DateTime date)
        : CustomException($"Parking spot: {name} is already reserved at: {date:d}.")
    {
        public string Name { get; } = name;
        public DateTime Date { get; } = date;
    }
}
