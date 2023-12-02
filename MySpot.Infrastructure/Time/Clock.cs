using MySpot.Application.Time;

namespace MySpot.Infrastructure.Time
{
    public class Clock : IClock
    {
        public DateTime Current() => DateTime.UtcNow;
    }
}
