namespace MySpot.Api.Exceptions
{
    public sealed class InvalidReservationDateException(DateTime date) 
        : CustomException($"Reservation date {date:d} is invalid.")
    {
        public DateTime Date { get; } = date;
    }
}
