using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects
{
    public sealed record Role
    {
        public static IEnumerable<string> AvailableRoles { get; } = ["admin", "user"];

        public string Value { get; }

        public Role(string value)
        {
            value = value.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(value) || 
                value.Length > 30 || 
                !AvailableRoles.Contains(value))
                throw new InvalidRoleException(value);

            Value = value;
        }

        public static Role Admin() => new("admin");
        public static Role User() => new("user");

        public static implicit operator Role(string value) => new(value);
        public static implicit operator string(Role role) => role.Value;
    }
}
