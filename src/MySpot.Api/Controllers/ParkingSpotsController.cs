using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Core.Entities;

namespace MySpot.Api.Controllers
{
    //[Route("[controller]")]
    [Route("parking-spots")]
    [ApiController]
    public class ParkingSpotsController(IQueryHandler<GetWeeklyParkingSpots, IEnumerable<WeeklyParkingSpotDto>> queryHandler)
        : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> Get([FromQuery] GetWeeklyParkingSpots query)
            => Ok(await queryHandler.HandleAsync(query));
    }
}
