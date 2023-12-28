using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects
{
    public sealed record Fullname
    {
        public string Value { get; }

        public Fullname(string value)
        {
            if (value.Length is > 100 or < 3)
                throw new InvalidFullnameException(value);

            Value = value;
        }

        public static implicit operator string(Fullname value) => value.Value;
        public static implicit operator Fullname(string value) => new(value);
    }
}
