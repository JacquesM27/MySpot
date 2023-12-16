using MySpot.Application.Commands;
using MySpot.Application.DTO;

namespace MySpot.Application.Services
{
    public interface IReservationsService
    {
        Task<Guid?> ReserveForVehicleAsync(ReserveParkingSpotForVehicle command);
        Task ReserveForCleaningAsync(ReserveParkingSpotForCleaning command);
        Task<bool> DeleteAsync(DeleteReservation command);
        Task<ReservationDto?> GetAsync(Guid id);
        Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync();
        Task<bool> ChangeReservationLicensePlateAsync(ChangeReservationLicensePlate command);
    }
}