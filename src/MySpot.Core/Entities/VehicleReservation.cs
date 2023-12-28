using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities
{
    public sealed class VehicleReservation : Reservation
    {
        public UserId UserId { get; set; }
        public EmployeeName EmployeeName { get; private set; }
        public LicensePlate LicensePlate { get; private set; }

        public VehicleReservation(ReservationId id, UserId userId, ParkingSpotId parkingSpotId, 
            EmployeeName employeeName, LicensePlate licensePlate, Capacity capacity, Date date) 
            : base(id, parkingSpotId, capacity, date)
        {
            UserId = userId;
            EmployeeName = employeeName;
            ChangeLicensePlate(licensePlate);
        }

        private VehicleReservation() { }

        public void ChangeLicensePlate(LicensePlate licensePlate)
            => LicensePlate = licensePlate;
    }
}
