namespace MySpot.Core.Exceptions
{
    public sealed class InvalidUsernameException(string username)
        : CustomException($"Username: '{username}' is invalid.")
    {
        public string Username => username;
    }
}
