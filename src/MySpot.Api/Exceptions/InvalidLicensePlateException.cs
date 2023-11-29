namespace MySpot.Api.Exceptions
{
    public sealed class InvalidLicensePlateException(string licensePlate) : CustomException($"License plate: {licensePlate} is invalid.")
    {
    }
}
