using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands
{
    public record ReserveParkingSpotForVehicle(Guid ParkingSpotId, Guid ReservationId, Guid UserId,
        int Capacity, DateTime Date, string EmployeeName, string LicensePlate) : ICommand;
}
