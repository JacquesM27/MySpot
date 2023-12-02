namespace MySpot.Core.Exceptions
{
    public sealed class InvalidLicensePlateException(string licensePlate) : CustomException($"License plate: {licensePlate} is invalid.")
    {
    }
}
