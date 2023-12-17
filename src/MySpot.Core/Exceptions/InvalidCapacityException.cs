namespace MySpot.Core.Exceptions
{
    public sealed class InvalidCapacityException(int capacity) 
        : CustomException($"Capacity {capacity} is invalid.")
    {
        public int Capacity => capacity;
    }
}
