using MySpot.Core.Exceptions;
using System.Text.RegularExpressions;

namespace MySpot.Core.ValueObjects
{
    public sealed record Email
    {
        private static readonly Regex Regex = new(
        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
        RegexOptions.Compiled);

        public string Value { get; }

        public Email(string value)
        {
            value = value.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(value) ||
                value.Length > 100 ||
                !Regex.IsMatch(value))
                throw new InvalidEmailException(value);

            Value = value;
        }

        public static implicit operator string(Email value) => value.Value;
        public static implicit operator Email(string value) => new(value);
    }
}
