using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySpot.Application.DTO
{
    public class WeeklyParkingSpotDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Capacity { get; set; }
        public IEnumerable<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();
    }
}
