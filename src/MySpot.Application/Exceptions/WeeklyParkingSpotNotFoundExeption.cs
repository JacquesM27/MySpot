using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions
{
    internal class WeeklyParkingSpotNotFoundExeption(Guid id) 
        : CustomException($"Weekly parking spot with id: {id} was not found.")
    {
        public Guid Id => id;
    }
}
