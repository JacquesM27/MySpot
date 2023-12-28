using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions
{
    public sealed class UsernameAlreadyInUseException(string username)
        : CustomException($"Username: '{username}' is already in use.")
    {
        public string Username => username;
    }
}
