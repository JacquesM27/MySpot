using MySpot.Core.Abstractions;

namespace MySpot.Unit.Tests.Shared
{
    public class TestClock : IClock
    {
        public DateTime Current() => new(2023, 12, 1, 12, 0, 0);
    }
}
