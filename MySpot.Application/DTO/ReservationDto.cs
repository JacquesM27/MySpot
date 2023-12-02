namespace MySpot.Application.DTO
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid ParkingSpotId { get; set; }
        public string Employeename { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
