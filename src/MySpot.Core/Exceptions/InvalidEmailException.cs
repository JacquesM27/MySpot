namespace MySpot.Core.Exceptions
{
    public sealed class InvalidEmailException(string email) 
        : CustomException($"Email: '{email}' is invalid.")
    {
        public string Email => email;
    }
}
