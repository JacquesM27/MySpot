using MySpot.Api.Commands;
using MySpot.Api.DTO;

namespace MySpot.Api.Services
{
    public interface IReservationsService
    {
        Guid? Create(CreateReservation command);
        bool Delete(DeleteReservation command);
        ReservationDto? Get(Guid id);
        IEnumerable<ReservationDto> GetAllWeekly();
        bool Update(ChangeReservationLicensePlate command);
    }
}