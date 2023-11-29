using MySpot.Api.Exceptions;

namespace MySpot.Api.ValueObjects
{
    public sealed record EmployeeName(string Value)
    {
        public string Value { get; } = Value ?? throw new InvalidEmployeeNameException();

        public static implicit operator EmployeeName(string Value) => new(Value);

        public static implicit operator string(EmployeeName Value) => Value.Value;
    }
}
