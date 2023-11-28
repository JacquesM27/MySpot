using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Entities;
using MySpot.Api.Services;
using System.Net;

namespace MySpot.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationsService _service = new();

        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> Get() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        public ActionResult<Reservation>? Get(int id)
        {
            var reservation = _service.Get(id);
            if (reservation is null)
                return NotFound();

            return Ok(reservation);
        }

        [HttpPost]
        public ActionResult Post(Reservation reservation)
        {
            var id = _service.Create(reservation);
            if(id is null) 
                return BadRequest();
            return CreatedAtAction(nameof(Get), new { id }, null);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Reservation reservation)
        {
            reservation.Id = id;
            if (_service.Update(reservation))
                return NoContent();

            return NotFound();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            if (_service.Delete(id))
                return NoContent();
            return NotFound();
        }
    }
}
