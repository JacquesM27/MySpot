using MySpot.Api.Services;

namespace MySpot.Unit.Tests.Shared
{
    public class TestClock : IClock
    {
        public DateTime Current() => new(2023, 12, 1);
    }
}
