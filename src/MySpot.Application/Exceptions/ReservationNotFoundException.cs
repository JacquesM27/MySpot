using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions
{
    public sealed class ReservationNotFoundException(Guid id) 
        : CustomException($"Reservation with Id: {id} was not found.")
    {
        public Guid Id => id;
    }
}
