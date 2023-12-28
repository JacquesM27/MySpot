namespace MySpot.Core.Exceptions
{
    public sealed class InvalidFullnameException(string fullname)
        : CustomException($"Fullname: '{fullname}' is invalid.")
    {
        public string Fullname => fullname;
    }
}
