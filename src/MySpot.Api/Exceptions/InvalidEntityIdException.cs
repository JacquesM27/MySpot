namespace MySpot.Api.Exceptions
{
    public sealed class InvalidEntityIdException(object id) : CustomException($"Cannot set: {id} as entity identifier.")
    {
        public object Id { get; } = id;
    }
}
