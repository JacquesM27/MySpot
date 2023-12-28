using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions
{
    public sealed class UserNotFoundException(Guid userId)
        : CustomException($"User with Id: '{userId}' was not found.")
    {
        public Guid UserId => userId;
    }
}
