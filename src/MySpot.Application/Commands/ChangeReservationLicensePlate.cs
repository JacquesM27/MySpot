namespace MySpot.Application.Commands
{
    public sealed record ChangeReservationLicensePlate(Guid ReservationId, string LicensePlate);
}
