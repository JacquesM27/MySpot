using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;

namespace MySpot.Api.Controllers
{
    [Route("parking-spots")]
    [ApiController]
    public class ReservationsController(ICommandHandler<ReserveParkingSpotForVehicle> reserveForVehicleHandler,
                                        ICommandHandler<ReserveParkingSpotForCleaning> reserveForCleaningHandler,
                                        ICommandHandler<ChangeReservationLicensePlate> changeLicensePlateHandler,
                                        ICommandHandler<DeleteReservation> deleteHandler) 
        : ControllerBase
    {

        [HttpPost("{parkingSpotId:guid}/reservations/vehicle")]
        public async Task<ActionResult> Post(Guid parkingSpotId, ReserveParkingSpotForVehicle command)
        {
            await reserveForVehicleHandler.HandleAsync(command with
            {
                ReservationId = Guid.NewGuid(),
                ParkingSpotId = parkingSpotId
            });
            return NoContent();
        }

        [HttpPost("reservations/cleaning")]
        public async Task<ActionResult> Post(ReserveParkingSpotForCleaning command)
        {
            await reserveForCleaningHandler.HandleAsync(command);
            return NoContent();
        }

        [HttpPut("reservations/{reservationId:guid}")]
        public async Task<ActionResult> Put(Guid reservationId, ChangeReservationLicensePlate command)
        {
            await changeLicensePlateHandler.HandleAsync(command with
            {
                ReservationId = reservationId
            });
            return NoContent();
        }

        [HttpDelete("reservations/{reservationId:guid}")]
        public async Task<ActionResult> Delete(Guid reservationId)
        {
            await deleteHandler.HandleAsync(new DeleteReservation(reservationId));
            return NoContent();
        }
    }
}
