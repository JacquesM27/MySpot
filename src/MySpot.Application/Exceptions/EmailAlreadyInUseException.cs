using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions
{
    public sealed class EmailAlreadyInUseException(string email)
        : CustomException($"Email: '{email}' is already in use.")
    {
        public string Email => email;
    }
}
