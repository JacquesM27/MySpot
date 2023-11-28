namespace MySpot.Api.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string ParkingSpotName { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
